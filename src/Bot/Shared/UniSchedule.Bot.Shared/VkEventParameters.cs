using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace UniSchedule.Bot.Shared;

/// <summary>
///     Входная модель события VK
/// </summary>
public class VkEventParameters
{
    /// <summary>
    ///     Тип события
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    ///     Версия Vk API
    /// </summary>
    [JsonPropertyName("v")]
    [JsonProperty("v")]
    public string? Version { get; set; }

    /// <summary>
    ///     Объект, инициировавший событие
    /// </summary>
    public Dictionary<string, object>? Object { get; set; }

    /// <summary>
    ///     Идентификатор сообщества, в котором произошло событие
    /// </summary>
    [JsonProperty("group_id")]
    public long GroupId { get; set; }

    /// <summary>
    ///     Секретное слово
    /// </summary>
    public string Secret { get; set; }

    public override string ToString()
    {
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
        };
        
        return JsonConvert.SerializeObject(this, settings);
    }
}