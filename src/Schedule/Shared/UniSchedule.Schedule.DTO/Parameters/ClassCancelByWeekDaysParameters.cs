namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры отмены пар по дням недели и типу недели
/// </summary>
public class ClassCancelByWeekDaysParameters
{
    /// <summary>
    ///     Дни недели для отмены на четных неделях
    /// </summary>
    public List<DayOfWeek>? Even { get; set; }

    /// <summary>
    ///     Дни недели для отмены на нечетных неделях
    /// </summary>
    public List<DayOfWeek>? Odd { get; set; }
}
