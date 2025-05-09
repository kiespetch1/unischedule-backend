namespace UniSchedule.Identity.DTO.Messages.Groups;

/// <summary>
///     Параметры для удаления пользователя в других сервисах через брокер сообщений
/// </summary>
public class GroupMqDeleteParameters
{
    /// <summary>
    ///     Идентификатор пользователя
    /// </summary>
    public Guid Id { get; set; }
}