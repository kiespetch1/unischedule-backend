using UniSchedule.Abstractions.Queries;
using UniSchedule.Shared.DTO.Models;

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
    ///     Информация о получателях
    /// </summary>
    public AnnouncementTargetModel? Target { get; set; }
}