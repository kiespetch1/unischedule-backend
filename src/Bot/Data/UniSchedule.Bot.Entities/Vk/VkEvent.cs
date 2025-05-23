using System.Text.Json.Serialization;

namespace UniSchedule.Bot.Entities.Vk;

public class VkEvent
{
    /// <summary>
    ///     Тип события
    /// </summary>
    public VkResponseType Type { get; set; }

    /// <summary>
    ///     Версия Vk API
    /// </summary>
    [JsonPropertyName("v")]
    public string? Version { get; set; }

    /// <summary>
    ///     Объект, инициировавший событие
    /// </summary>
    public Dictionary<string, object>? Object { get; set; }

    /// <summary>
    ///     Идентификатор сообщества, в котором произошло событие
    /// </summary>
    public long GroupId { get; set; }
}