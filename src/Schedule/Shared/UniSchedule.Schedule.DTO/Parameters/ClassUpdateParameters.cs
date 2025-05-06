using UniSchedule.Schedule.Entities.Enums;

namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры обновления пары
/// </summary>
public class ClassUpdateParameters
{
    /// <summary>
    ///     Название
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     Время начала
    /// </summary>
    public required TimeOnly StartedAt { get; set; }

    /// <summary>
    ///     Время окончания
    /// </summary>
    public required TimeOnly FinishedAt { get; set; }

    /// <summary>
    ///     Тип пары (лекция/практика/лабораторная)
    /// </summary>
    public ClassType Type { get; set; }

    /// <summary>
    ///     Тип недели, на которой проводится пара (четная/нечетная)
    /// </summary>
    public required WeekType WeekType { get; set; }

    /// <summary>
    ///     Подгруппа, для которой проводится пара
    /// </summary>
    public required Subgroup Subgroup { get; set; }

    /// <summary>
    ///     Идентификатор места проведения
    /// </summary>
    public required Guid LocationId { get; set; }

    /// <summary>
    ///     Идентификатор преподавателя
    /// </summary>
    public required Guid TeacherId { get; set; }
}