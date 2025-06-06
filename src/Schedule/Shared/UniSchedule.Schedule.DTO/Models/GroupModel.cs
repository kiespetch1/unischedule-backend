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
    ///     Имеет ли четкое разделение на подгруппы
    /// </summary>
    public required bool HasFixedSubgroups { get; set; }

    /// <summary>
    ///     Последнее объявление данной группы
    /// </summary>
    public AnnouncementsBlockModel? AnnouncementsBlock { get; set; }

    /// <summary>
    ///     Номер последней учебной недели
    /// </summary>
    public int LastAcademicWeekNumber { get; set; }

    /// <summary>
    ///     Недели группы
    /// </summary>
    public List<WeekModel> Weeks { get; set; }
}