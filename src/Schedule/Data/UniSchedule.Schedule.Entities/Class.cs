using System.ComponentModel.DataAnnotations.Schema;
using UniSchedule.Abstractions.Entities;
using UniSchedule.Schedule.Entities.Enums;

namespace UniSchedule.Schedule.Entities;

/// <summary>
///     Пара
/// </summary>
public class Class : Entity<Guid>
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
    ///     Тип (лекция, практика, лабораторная работа)
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
    ///     Отменена ли пара
    /// </summary>
    public required bool IsCancelled { get; set; }

    /// <summary>
    ///     Идентификатор дня
    /// </summary>
    public Guid DayId { get; set; }

    /// <summary>
    ///     День
    /// </summary>
    public Day Day { get; set; }

    /// <summary>
    ///     Идентификатор места проведения
    /// </summary>
    public required Guid LocationId { get; set; }

    /// <summary>
    ///     Место проведения
    /// </summary>
    public Location Location { get; set; }

    /// <summary>
    ///     Идентификатор преподавателя
    /// </summary>
    public required Guid TeacherId { get; set; }

    /// <summary>
    ///     Преподаватель
    /// </summary>
    public Teacher Teacher { get; set; }

    /// <summary>
    ///     Скрыта ли пара при показе расписания
    /// </summary>
    [NotMapped]
    public bool IsHidden { get; set; }
}