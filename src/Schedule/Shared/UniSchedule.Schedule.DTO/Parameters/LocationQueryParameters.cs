using UniSchedule.Abstractions.Queries;
using UniSchedule.Schedule.Entities.Enums;

namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры запроса мест проведения
/// </summary>
public class LocationQueryParameters : QueryParameters
{
    /// <summary>
    ///     Тип места проведения
    /// </summary>
    public LocationType? LocationType { get; set; }
}