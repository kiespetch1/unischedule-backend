﻿using UniSchedule.Abstractions.Entities;
using UniSchedule.Schedule.Entities.Enums;

namespace UniSchedule.Schedule.Entities;

/// <summary>
///     Учебная группа
/// </summary>
public class Group : Entity<Guid>
{
    /// <summary>
    ///     Название
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     Курс
    /// </summary>
    public required int Grade { get; set; }

    /// <summary>
    ///     Тип недели
    /// </summary>
    public required WeekType WeekType { get; set; }

    /// <summary>
    ///     Имеет ли разделение по подгруппам
    /// </summary>
    public required bool HasSubgroups { get; set; }

    /// <summary>
    ///     Имеет ли четкое разделение на подгруппы
    /// </summary>
    public required bool HasFixedSubgroups { get; set; }
}