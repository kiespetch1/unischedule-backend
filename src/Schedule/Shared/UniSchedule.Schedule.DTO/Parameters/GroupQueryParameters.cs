using UniSchedule.Abstractions.Queries;

namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры запроса групп
/// </summary>
public class GroupQueryParameters : QueryParameters
{
    /// <summary>
    ///     Идентификаторы групп
    /// </summary>
    public List<Guid>? Ids { get; set; }
    
    /// <summary>
    ///     Курс
    /// </summary>
    public int? Grade { get; set; }

    /// <summary>
    ///     Имеет ли четкое разделение на подгруппы
    /// </summary>
    public bool? HasFixedSubgroups { get; set; }

    /// <summary>
    ///     Подгружать ли вложенные сущности
    /// </summary>
    public bool FetchDetails { get; set; } = true;
}