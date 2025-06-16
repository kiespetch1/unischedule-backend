using UniSchedule.Schedule.Entities.Enums;

namespace UniSchedule.Shared.DTO.Parameters;

public class ScheduleFilteringCreateParameters
{
    /// <summary>
    ///     Название пары
    /// </summary>
    public required string ClassName { get; set; }

    /// <summary>
    ///     Подгруппа
    /// </summary>
    public Subgroup Subgroup { get; set; }
}