namespace UniSchedule.Bot.Entities.Vk;

public class VkMessage
{
    /// <summary>
    ///     Идентификатор сообщения
    /// </summary>
    public long Id { get; set; } 
    
    /// <summary>
    ///     Дата и время сообщения (в UnixTime)
    /// </summary>
    public long Date { get; set; }

    /// <summary>
    ///     Идентификатор назначения (беседы)
    /// </summary>
    public long PeerId { get; set; }

    /// <summary>
    ///     Идентификатор отправителя
    /// </summary>
    public long FromId { get; set; }

    /// <summary>
    ///     Текст сообщения
    /// </summary>
    public string Text { get; set; }
}