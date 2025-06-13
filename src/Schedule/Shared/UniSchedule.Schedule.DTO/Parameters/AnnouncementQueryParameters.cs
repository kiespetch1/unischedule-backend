using UniSchedule.Abstractions.Queries;

namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры запроса объявлений
/// </summary>
public class AnnouncementQueryParameters : QueryParameters
{
    /// <summary>
    ///     Идентификатор создателя
    /// </summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>
    ///     Является ли анонимным
    /// </summary>
    public bool? IsAnonymous { get; set; }

    /// <summary>
    ///     Идентификатор группы целевой аудитории
    /// </summary>
    public Guid? GroupId { get; set; } 
}