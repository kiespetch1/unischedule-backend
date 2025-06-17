using UniSchedule.Abstractions.Queries;

namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры запроса данных LMS
/// </summary>
public class LmsDataQueryParameters : QueryParameters
{
    /// <summary>
    ///     Идентификатор группы
    /// </summary>
    public Guid? GroupId { get; set; }
}