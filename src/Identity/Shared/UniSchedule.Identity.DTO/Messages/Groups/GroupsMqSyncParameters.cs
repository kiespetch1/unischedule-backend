namespace UniSchedule.Identity.DTO.Messages.Groups;

/// <summary>
///     Параметры для синхронизации групп через брокера сообщений
/// </summary>
public class GroupsMqSyncParameters
{
    /// <summary>
    ///     Список групп для синхронизации
    /// </summary>
    public List<GroupMqModel> Groups { get; set; }
}