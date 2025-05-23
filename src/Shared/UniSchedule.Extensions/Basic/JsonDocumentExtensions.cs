using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace UniSchedule.Extensions.Basic;

/// <summary>
///     Методы расширения для JsonDocument
/// </summary>
public static class JsonDocumentExtensions
{
    #region [ Validation ]

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
            _ => value
        };
    }

    #endregion
}