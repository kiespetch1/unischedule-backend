using UniSchedule.Abstractions.Queries;
using UniSchedule.Schedule.Entities.Enums;

namespace UniSchedule.Shared.DTO.Parameters;

public class WeekQueryParameters : QueryParameters
{
    /// <summary>
    ///     Идентификаторы групп
    /// </summary>
    public List<Guid>? GroupIds { get; set; }

    /// <summary>
    ///     Подгруппа
    /// </summary>
    public Subgroup? Subgroup { get; set; }

    /// <summary>
    ///     Тип недели
    /// </summary>
    public WeekType? Type { get; set; }
}