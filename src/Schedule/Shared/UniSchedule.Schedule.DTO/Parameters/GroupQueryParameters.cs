using UniSchedule.Abstractions.Queries;

namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры запроса групп
/// </summary>
public class GroupQueryParameters : QueryParameters
{
    /// <summary>
    ///     Курс
    /// </summary>
    public int? Grade { get; set; }

    /// <summary>
    ///     Имеет ли разделение по подгруппам
    /// </summary>
    public bool? HasSubgroups { get; set; }

    /// <summary>
    ///     Имеет ли четкое разделение на подгруппы
    /// </summary>
    public bool? HasFixedSubgroups { get; set; }
}