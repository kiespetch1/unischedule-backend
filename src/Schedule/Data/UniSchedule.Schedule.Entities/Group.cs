using System.ComponentModel.DataAnnotations.Schema;
using UniSchedule.Abstractions.Entities;

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
    ///     Имеет ли разделение по подгруппам
    /// </summary>
    public required bool HasSubgroups { get; set; }

    /// <summary>
    ///     Имеет ли четкое разделение на подгруппы
    /// </summary>
    public required bool HasFixedSubgroups { get; set; }

    /// <summary>
    ///     Последнее объявление группы
    /// </summary>
    [NotMapped]
    public Announcement? LastAnnouncement { get; set; }

    /// <summary>
    ///     Недели группы
    /// </summary>
    public ICollection<Week> Weeks { get; set; }
}