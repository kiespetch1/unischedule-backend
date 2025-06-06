using System.ComponentModel.DataAnnotations.Schema;
using UniSchedule.Abstractions.Entities;
using UniSchedule.Entities;
using UniSchedule.Schedule.Entities.Owned;

namespace UniSchedule.Schedule.Entities;

/// <summary>
///     Учебная группа
/// </summary>
public class Group : Entity<Guid>
{
    /// <summary>
    ///     Название
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     Курс
    /// </summary>
    public required int Grade { get; set; }

    /// <summary>
    ///     Имеет ли фиксированное разделение на подгруппы
    /// </summary>
    public required bool HasFixedSubgroups { get; set; }

    /// <summary>
    ///     Данные блока объявления
    /// </summary>
    [NotMapped]
    public AnnouncementsBlock? AnnouncementsBlock { get; set; }

    /// <summary>
    ///     Номер последней учебной недели (без учета зачетных и экзаменационных)
    /// </summary>
    public int LastAcademicWeekNumber { get; set; } = 16;

    /// <summary>
    ///     Недели группы
    /// </summary>
    public ICollection<Week> Weeks { get; set; }

    /// <summary>
    ///     Используемый мессенджер
    /// </summary>
    public MessengerType UsedMessenger { get; set; } = MessengerType.None;
}