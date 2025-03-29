using UniSchedule.Schedule.Entities.Enums;

namespace UniSchedule.Shared.DTO.Models;

/// <summary>
///     Модель пары
/// </summary>
public class ClassModel
{
    /// <summary>
    ///     Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Название
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Время начала
    /// </summary>
    public TimeOnly StartedAt { get; set; }

    /// <summary>
    ///     Время окончания
    /// </summary>
    public TimeOnly FinishedAt { get; set; }

    /// <summary>
    ///     Тип недели, на которой проводится пара (четная/нечетная)
    /// </summary>
    public WeekType WeekType { get; set; }

    /// <summary>
    ///     Подгруппа, для которой проводится пара
    /// </summary>
    public Subgroup Subgroup { get; set; }

    /// <summary>
    ///     Отменена ли пара
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    ///     Идентификатор дня
    /// </summary>
    public Guid DayId { get; set; }

    /// <summary>
    ///     Идентификатор места проведения
    /// </summary>
    public Guid LocationId { get; set; }

    /// <summary>
    ///     Идентификатор преподавателя
    /// </summary>
    public Guid TeacherId { get; set; }
}