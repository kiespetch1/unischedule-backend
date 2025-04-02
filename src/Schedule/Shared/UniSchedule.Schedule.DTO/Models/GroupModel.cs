using UniSchedule.Schedule.Entities.Enums;

namespace UniSchedule.Shared.DTO.Models;

/// <summary>
///     Модель группы
/// </summary>
public class GroupModel
{
    /// <summary>
    ///     Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Название
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     Курс
    /// </summary>
    public required int Grade { get; set; }

    /// <summary>
    ///     Тип недели
    /// </summary>
    public required WeekType WeekType { get; set; }

    /// <summary>
    ///     Имеет ли разделение по подгруппам
    /// </summary>
    public required bool HasSubgroups { get; set; }

    /// <summary>
    ///     Имеет ли четкое разделение на подгруппы
    /// </summary>
    public required bool HasFixedSubgroups { get; set; }

    /// <summary>
    ///     Последнее объявление данной группы
    /// </summary>
    public AnnouncementModel? LastAnnouncement { get; set; }

    /// <summary>
    ///     Недели группы
    /// </summary>
    public List<WeekModel> Weeks { get; set; }
}