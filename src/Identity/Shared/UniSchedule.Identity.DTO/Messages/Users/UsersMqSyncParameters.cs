namespace UniSchedule.Identity.DTO.Messages.Users;

/// <summary>
///     Параметры для синхронизации пользователей через брокера сообщений
/// </summary>
public class UsersMqSyncParameters
{
    /// <summary>
    ///     Список пользователей для синхронизации
    /// </summary>
    public List<UserMqModel> Users { get; set; }
}