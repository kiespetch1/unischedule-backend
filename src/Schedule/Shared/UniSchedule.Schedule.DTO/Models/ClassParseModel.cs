using UniSchedule.Schedule.Entities.Enums;

namespace UniSchedule.Shared.DTO.Models;

public class ClassParseModel
{
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
    ///     Тип (лекция, практика, лабораторная работа)
    /// </summary>
    public ClassType Type { get; set; }

    /// <summary>
    ///     Тип недели, на которой проводится пара (четная/нечетная)
    /// </summary>
    public WeekType WeekType { get; set; }

    /// <summary>
    ///     Подгруппа, для которой проводится пара
    /// </summary>
    public Subgroup Subgroup { get; set; }

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