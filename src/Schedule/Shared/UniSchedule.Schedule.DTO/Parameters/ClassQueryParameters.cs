using UniSchedule.Abstractions.Queries;
using UniSchedule.Schedule.Entities.Enums;

namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры запроса пар
/// </summary>
public class ClassQueryParameters : QueryParameters
{
    /// <summary>
    ///     Идентификатор дня
    /// </summary>
    public Guid? DayId { get; set; }

    /// <summary>
    ///     Идентификатор преподавателя
    /// </summary>
    public Guid? TeacherId { get; set; }

    /// <summary>
    ///     Идентификатор места проведения
    /// </summary>
    public Guid? LocationId { get; set; }

    /// <summary>
    ///     Тип недели, на которой проводится пара (четная/нечетная)
    /// </summary>
    public WeekType? WeekType { get; set; }

    /// <summary>
    ///     Подгруппа, для которой проводится пара
    /// </summary>
    public Subgroup? Subgroup { get; set; }

    /// <summary>
    ///     Отменена ли пара
    /// </summary>
    public bool? IsCancelled { get; set; }
}