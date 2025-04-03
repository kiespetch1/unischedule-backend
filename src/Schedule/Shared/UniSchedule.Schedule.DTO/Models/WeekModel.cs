using UniSchedule.Schedule.Entities.Enums;

namespace UniSchedule.Shared.DTO.Models;

/// <summary>
///     Модель недели
/// </summary>
public class WeekModel
{
    /// <summary>
    ///     Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Идентификатор группы
    /// </summary>
    public Guid GroupId { get; set; }

    /// <summary>
    ///     Тип недели (четная/нечетная/все)
    /// </summary>
    public WeekType Type { get; set; }

    /// <summary>
    ///     Подгруппа
    /// </summary>
    public Subgroup Subgroup { get; set; }

    /// <summary>
    ///     Дни
    /// </summary>
    public ICollection<DayModel> Days { get; set; }
}