namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры отмены нескольких пар по идентификаторам дней
/// </summary>
public class ClassMultipleCancelByDayIdParameters
{
    /// <summary>
    ///     Идентификаторы дней
    /// </summary>
    public List<Guid> DayIds { get; set; }
}