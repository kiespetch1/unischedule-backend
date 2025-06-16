using UniSchedule.Abstractions.Queries;

namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры запроса фильтрации по пользователю
/// </summary>
public class ScheduleFilteringQueryParameters : QueryParameters
{
    /// <summary>
    ///     Идентификатор пользователя
    /// </summary>
    public Guid? UserId { get; set; }
}