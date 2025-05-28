using UniSchedule.Abstractions.Entities;

namespace UniSchedule.Bot.Entities.Auxiliary;

/// <summary>
///     Связь между идентификатором группы и беседы мессенджера для группы
/// </summary>
public class GroupMessengerConversation : Entity<Guid>
{
    /// <summary>
    ///     Идентификатор группы в системе
    /// </summary>
    public Guid GroupId { get; set; }

    /// <summary>
    ///     Идентификатор беседы в мессенджере
    /// </summary>
    public long ConversationId { get; set; }

    /// <summary>
    ///     Название беседы
    /// </summary>
    public string ConversationName { get; set; }
}