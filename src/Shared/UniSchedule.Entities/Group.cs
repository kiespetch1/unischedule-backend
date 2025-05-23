using UniSchedule.Abstractions.Entities;

namespace UniSchedule.Entities;

/// <summary>
///     Учебная группа
/// </summary>
public class Group : Entity<Guid>
{
    /// <summary>
    ///     Название группы
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Тип используемого мессенджера
    /// </summary>
    public MessengerType UsedMessenger { get; set; } = MessengerType.None;
}