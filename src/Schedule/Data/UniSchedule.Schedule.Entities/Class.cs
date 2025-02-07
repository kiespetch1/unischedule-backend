﻿using UniSchedule.Abstractions.Entities;
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
    ///     День
    /// </summary>
    public Day Day { get; set; }

    /// <summary>
    ///     Идентификатор места проведения
    /// </summary>
    public Guid LocationId { get; set; }

    /// <summary>
    ///     Место проведения
    /// </summary>
    public Location Location { get; set; }

    /// <summary>
    ///     Идентификатор преподавателя
    /// </summary>
    public Guid TeacherId { get; set; }

    /// <summary>
    ///     Преподаватель
    /// </summary>
    public Teacher Teacher { get; set; }
}