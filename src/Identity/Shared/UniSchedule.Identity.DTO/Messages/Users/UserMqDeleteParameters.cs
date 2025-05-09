namespace UniSchedule.Identity.DTO.Messages.Users;

/// <summary>
///     Параметры для удаления пользователя в других сервисах через брокер сообщений
/// </summary>
public class UserMqDeleteParameters
{
    /// <summary>
    ///     Идентификатор пользователя
    /// </summary>
    public Guid Id { get; set; }
}