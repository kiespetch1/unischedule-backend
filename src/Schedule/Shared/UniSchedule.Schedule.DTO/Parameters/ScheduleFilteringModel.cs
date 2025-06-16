using UniSchedule.Schedule.Entities.Enums;

namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Модель персональной фильтрации расписания
/// </summary>
public class ScheduleFilteringModel
{
    /// <summary>
    ///     Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Название пары
    /// </summary>
    public required string ClassName { get; set; }

    /// <summary>
    ///     Подгруппа
    /// </summary>
    public Subgroup Subgroup { get; set; }
}