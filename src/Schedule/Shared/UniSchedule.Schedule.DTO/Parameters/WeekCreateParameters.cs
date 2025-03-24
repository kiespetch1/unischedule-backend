using UniSchedule.Schedule.Entities.Enums;

namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры создания недели
/// </summary>
public class WeekCreateParameters
{
    /// <summary>
    ///     Тип недели (четная/нечетная)
    /// </summary>
    public WeekType WeekType { get; set; }

    /// <summary>
    ///     Подгруппа
    /// </summary>
    public Subgroup Subgroup { get; set; }

    /// <summary>
    ///     Идентификатор группы
    /// </summary>
    public Guid GroupId { get; set; }
}