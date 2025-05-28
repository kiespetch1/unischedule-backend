namespace UniSchedule.Bot.Shared;

/// <summary>
///     Параметры для привязки бота к беседе в мессенджере
/// </summary>
public class MessengerLinkParameters
{
    /// <summary>
    ///     Идентификатор пользователя в мессенджере
    /// </summary>
    public required long MessengerUserId { get; set; }

    /// <summary>
    ///     Идентификатор беседы
    /// </summary>
    public required long ConversationId { get; set; }

    /// <summary>
    ///     Название беседы
    /// </summary>
    public string ConversationName { get; set; }
}