using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;

namespace UniSchedule.Extensions.Basic;

/// <summary>
///     Методы расширения для JsonDocument
/// </summary>
public static class JsonDocumentExtensions
{
    /// <summary>
    ///     Валидация на наличие полей
    /// </summary>
    /// <param name="input">Входная модель</param>
    /// <param name="template">Шаблон с указанием наличия полей</param>
    /// <returns>Валидность модели</returns>
    public static bool Validate(this Dictionary<string, object> input, JsonDocument template)
    {
        var inputDict = JObjectToDictionary(input);
        var templateDict = JsonDocumentToDictionary(template);
        return ValidateDictionary(inputDict, templateDict);
    }

    private static bool ValidateDictionary(Dictionary<string, object> input, Dictionary<string, object> template)
    {
        foreach (var key in template.Keys)
        {
            switch (template[key])
            {
                case true when !input.ContainsKey(key):
                    return false;
                case Dictionary<string, object> nestedTemplate:
                {
                    if (!input.ContainsKey(key) || !(input[key] is Dictionary<string, object> nestedInput))
                    {
                        return false;
                    }

                    if (!ValidateDictionary(nestedInput, nestedTemplate))
                    {
                        return false;
                    }

                    break;
                }
            }
        }

        return true;
    }

    private static Dictionary<string, object> JsonDocumentToDictionary(JsonDocument doc)
    {
        var dict = new Dictionary<string, object>();
        foreach (var property in doc.RootElement.EnumerateObject())
        {
            dict[property.Name] = JsonElementToObject(property.Value)!;
        }

        return dict;
    }

    private static object? JsonElementToObject(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                var objDict = new Dictionary<string, object>();
                foreach (var property in element.EnumerateObject())
                {
                    objDict[property.Name] = JsonElementToObject(property.Value)!;
                }

                return objDict;
            case JsonValueKind.Array:
                var array = new List<object>();
                foreach (var item in element.EnumerateArray())
                {
                    array.Add(JsonElementToObject(item)!);
                }

                return array;
            case JsonValueKind.String:
                return element.GetString()!;
            case JsonValueKind.Number:
                if (element.TryGetInt32(out var intValue))
                {
                    return intValue;
                }

                if (element.TryGetDouble(out var doubleValue))
                {
                    return doubleValue;
                }

                if (element.TryGetInt64(out var longValue))
                {
                    return longValue;
                }

                if (element.TryGetDecimal(out var decimalValue))
                {
                    return decimalValue;
                }

                break;
            case JsonValueKind.True:
                return true;
            case JsonValueKind.False:
                return false;
            case JsonValueKind.Null:
                return null;
        }

        return null;
    }


    /// <summary>
    ///     Рекурсивное преобразование словаря со значениями типа JObject в словарь со значениями любого типа
    /// </summary>
    /// <param name="input">Изначальный словарь</param>
    /// <returns>Преобразованный словарь</returns>
    public static Dictionary<string, object> JObjectToDictionary(Dictionary<string, object>? input)
    {
        if (input == null)
        {
            return new Dictionary<string, object>();
        }

        var result = new Dictionary<string, object>();

        foreach (var kvp in input)
        {
            result[kvp.Key] = ConvertJToken(kvp.Value);
        }

        return result;
    }

    private static object ConvertJToken(object value)
    {
        return value switch
        {
            JObject jObject => JObjectToDictionary(jObject.ToObject<Dictionary<string, object>>()!),
            JArray jArray => jArray.Select(ConvertJToken).ToList(),
            JValue jValue => jValue.Value!,
            JsonElement jsonElement => ConvertJsonElementToObject(jsonElement),
            _ => value
        };
    }

    private static object ConvertJsonElementToObject(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                var dict = new Dictionary<string, object>();
                foreach (var property in element.EnumerateObject())
                {
                    dict[property.Name] = ConvertJsonElementToObject(property.Value);
                }
                return dict;
            
            case JsonValueKind.Array:
                var list = new List<object>();
                foreach (var item in element.EnumerateArray())
                {
                    list.Add(ConvertJsonElementToObject(item));
                }
                return list;
            
            case JsonValueKind.String:
                return element.GetString()!;
            
            case JsonValueKind.Number:
                if (element.TryGetInt32(out var intValue))
                {
                    return intValue;
                }

                if (element.TryGetInt64(out var longValue))
                {
                    return longValue;
                }

                if (element.TryGetDouble(out var doubleValue))
                {
                    return doubleValue;
                }

                if (element.TryGetDecimal(out var decimalValue))
                {
                    return decimalValue;
                }

                return element.GetDouble();
            
            case JsonValueKind.True:
                return true;
            
            case JsonValueKind.False:
                return false;
            
            case JsonValueKind.Null:
            case JsonValueKind.Undefined:
            default:
                return null!;
        }
    }

    /// <summary>
    /// Рекурсивно преобразует <see cref="Dictionary{TKey,TValue}"/> произвольной вложенности в <see cref="JsonDocument"/>.
    /// </summary>
    public static JsonDocument ToJsonDocument(this Dictionary<string, object> dictionary)
    {
        var root = BuildNode(dictionary) as JsonObject
                   ?? throw new InvalidOperationException("Корень JSON-документа обязан быть объектом");

        return JsonDocument.Parse(root.ToJsonString());
    }

    // ---------- приватные хелперы ----------

    private static JsonNode? BuildNode(object? val)
    {
        if (val is null)
        {
            return null;
        }

        if (val is JsonNode n)
        {
            return n;
        }

        // ВАЖНО: обработка Newtonsoft.Json типов должна быть первой
        if (val is JToken jToken)
        {
            return ConvertJToken(jToken);
        }

        // ВАЖНО: проверка JsonElement должна быть ПЕРЕД проверкой на словарь,
        // так как JsonElement может реализовывать IDictionary
        if (val is JsonElement je)
        {
            return ConvertJsonElement(je);
        }

        if (val is JsonDocument jd)
        {
            return ConvertJsonElement(jd.RootElement);
        }

        // ---------- словари ----------
        if (IsDictionary(val) && val is not JsonElement)
        {
            var obj = new JsonObject();
            foreach (var (k, v) in EnumerateDictionary(val))
            {
                obj[k] = BuildNode(v);
            }
            return obj;
        }

        // ---------- строки ----------
        if (val is string s)
        {
            return TryParseJson(s) ?? JsonValue.Create(s);
        }

        // ---------- коллекции ----------
        if (val is IEnumerable en && val is not string)
        {
            var arr = new JsonArray();
            foreach (var item in en)
            {
                arr.Add(BuildNode(item));
            }
            return arr;
        }

        // ---------- примитивы и любые сериализуемые объекты ----------
        return JsonValue.Create(val);
    }

    private static JsonNode? ConvertJToken(JToken token)
    {
        switch (token.Type)
        {
            case JTokenType.Object:
                var obj = new JsonObject();
                foreach (var property in (JObject)token)
                {
                    obj[property.Key] = ConvertJToken(property.Value);
                }
                return obj;
            
            case JTokenType.Array:
                var arr = new JsonArray();
                foreach (var item in (JArray)token)
                {
                    arr.Add(ConvertJToken(item));
                }
                return arr;
            
            case JTokenType.String:
                return JsonValue.Create(token.Value<string>());
            
            case JTokenType.Integer:
                return JsonValue.Create(token.Value<long>());
            
            case JTokenType.Float:
                return JsonValue.Create(token.Value<double>());
            
            case JTokenType.Boolean:
                return JsonValue.Create(token.Value<bool>());
            
            case JTokenType.Null:
            case JTokenType.Undefined:
                return null;
            
            case JTokenType.Date:
                return JsonValue.Create(token.Value<DateTime>());
            
            default:
                return JsonValue.Create(token.ToString());
        }
    }

    private static JsonNode? ConvertJsonElement(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                var obj = new JsonObject();
                foreach (var property in element.EnumerateObject())
                {
                    obj[property.Name] = ConvertJsonElement(property.Value);
                }
                return obj;
            
            case JsonValueKind.Array:
                var arr = new JsonArray();
                foreach (var item in element.EnumerateArray())
                {
                    arr.Add(ConvertJsonElement(item));
                }
                return arr;
            
            case JsonValueKind.String:
                return JsonValue.Create(element.GetString());
            
            case JsonValueKind.Number:
                if (element.TryGetInt32(out var intValue))
                {
                    return JsonValue.Create(intValue);
                }

                if (element.TryGetInt64(out var longValue))
                {
                    return JsonValue.Create(longValue);
                }

                if (element.TryGetDouble(out var doubleValue))
                {
                    return JsonValue.Create(doubleValue);
                }

                if (element.TryGetDecimal(out var decimalValue))
                {
                    return JsonValue.Create(decimalValue);
                }

                return JsonValue.Create(element.GetRawText());
            
            case JsonValueKind.True:
                return JsonValue.Create(true);
            
            case JsonValueKind.False:
                return JsonValue.Create(false);
            
            case JsonValueKind.Null:
            case JsonValueKind.Undefined:
            default:
                return null;
        }
    }

    // Проверяем, реализует ли объект IDictionary<,>
    private static bool IsDictionary(object obj) =>
        obj is not JsonElement && obj is not JToken && (
            obj is IDictionary ||
            obj.GetType().GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>))
        );

    private static IEnumerable<(string key, object? val)> EnumerateDictionary(object dictObj)
    {
        if (dictObj is IDictionary legacy)
        {
            foreach (DictionaryEntry e in legacy)
            {
                if (e.Key is string k)
                {
                    yield return (k, e.Value);
                }
            }
            yield break;
        }

        var items = (IEnumerable)dictObj;
        foreach (var item in items)
        {
            var t = item.GetType();
            var keyProp = t.GetProperty("Key", BindingFlags.Public | BindingFlags.Instance);
            var valProp = t.GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);
            if (keyProp?.GetValue(item) is string k)
            {
                yield return (k, valProp?.GetValue(item));
            }
        }
    }

    private static JsonNode? TryParseJson(string text)
    {
        var trimmed = text.TrimStart();
        if (trimmed.Length == 0)
        {
            return null;
        }

        var first = trimmed[0];
        if (first is '{' or '[')
        {
            try
            {
                return JsonNode.Parse(text);
            }
            catch (JsonException)
            {
                // невалидный JSON – оставляем как строку
            }
        }

        return null;
    }
}