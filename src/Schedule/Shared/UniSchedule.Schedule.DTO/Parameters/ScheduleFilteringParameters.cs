namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры персональной фильтрации расписания
/// </summary>
public class ScheduleFilteringParameters
{
    /// <summary>
    ///     Список параметров фильтрации
    /// </summary>
    public required List<ScheduleFilteringModel> FilteringParameters { get; set; }
}