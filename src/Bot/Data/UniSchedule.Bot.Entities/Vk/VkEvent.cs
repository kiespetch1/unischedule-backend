using System.Text.Json;
using System.Text.Json.Serialization;

namespace UniSchedule.Bot.Entities.Vk;

/// <summary>
///     Событие VK
/// </summary>
public class VkEvent
{
    /// <summary>
    ///     Тип события
    /// </summary>
    [JsonIgnore]
    public VkResponseType Type { get; set; }

    /// <summary>
    ///     Версия Vk API
    /// </summary>
    [JsonPropertyName("v")]
    public string? Version { get; set; }

    /// <summary>
    ///     Объект, инициировавший событие
    /// </summary>
    public JsonDocument? Object { get; set; }

    /// <summary>
    ///     Идентификатор сообщества, в котором произошло событие
    /// </summary>
    public long GroupId { get; set; }

    public override string ToString() => JsonSerializer.Serialize(this);
}