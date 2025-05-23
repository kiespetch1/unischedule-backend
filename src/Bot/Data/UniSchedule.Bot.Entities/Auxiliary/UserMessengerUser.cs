using UniSchedule.Abstractions.Entities;

namespace UniSchedule.Bot.Entities.Auxiliary;

/// <summary>
///     Связь между идентификатором системы и мессенджера для пользователя
/// </summary>
public class UserMessengerUser : Entity<Guid>
{
    /// <summary>
    ///     Идентификатор пользователя в системе
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    ///     Идентификатор пользователя в мессенджере
    /// </summary>
    public long MessengerUserId { get; set; }
}