namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры создания дня недели
/// </summary>
public class DayCreateParameters
{
    /// <summary>
    ///     День недели
    /// </summary>
    public DayOfWeek DayOfWeek { get; set; }

    /// <summary>
    ///     Идентификатор недели
    /// </summary>
    public Guid WeekId { get; set; }
}