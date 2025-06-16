using UniSchedule.Abstractions.Entities;
using UniSchedule.Abstractions.Entities.Auditable;
using UniSchedule.Schedule.Entities.Enums;

namespace UniSchedule.Schedule.Entities;

/// <summary>
///     Опция персональной фильтрации расписания
/// </summary>
public class ScheduleFilteringOption : Entity<Guid>, ICreatable
{
    /// <summary>
    ///     Название пары
    /// </summary>
    public required string ClassName { get; set; }

    /// <summary>
    ///     Подгруппа
    /// </summary>
    public Subgroup Subgroup { get; set; }

    /// <summary>
    ///     Создатель записи
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///     Идентификатор создателя
    /// </summary>
    public Guid? CreatedBy { get; set; }
}