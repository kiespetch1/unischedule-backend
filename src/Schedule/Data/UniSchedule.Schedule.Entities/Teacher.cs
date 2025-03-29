﻿using UniSchedule.Abstractions.Entities;

namespace UniSchedule.Schedule.Entities;

/// <summary>
///     Преподаватель
/// </summary>
public class Teacher : Entity<Guid>
{
    /// <summary>
    ///     ФИО преподавателя
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     Полное ФИО преподавателя
    /// </summary>
    public required string FullName { get; set; }
}