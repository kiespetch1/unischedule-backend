﻿using System.ComponentModel.DataAnnotations.Schema;
using UniSchedule.Abstractions.Entities.Auditable;
using UniSchedule.Entities;
using UniSchedule.Schedule.Entities.Enums;
using UniSchedule.Schedule.Entities.Owned;

namespace UniSchedule.Schedule.Entities;

/// <summary>
///     Объявление
/// </summary>
public class Announcement : AuditableEntity<Guid>
{
    /// <summary>
    ///     Текст объявления
    /// </summary>
    public required string Message { get; set; }

    /// <summary>
    ///     Информация о получателях
    /// </summary>
    public required AnnouncementTargetInfo Target { get; set; }

    /// <summary>
    ///     Приоритет
    /// </summary>
    public required AnnouncementPriority Priority { get; set; }

    /// <summary>
    ///     Является ли анонимным
    /// </summary>
    public required bool IsAnonymous { get; set; }

    /// <summary>
    ///     Является ли доступным ограниченное время
    /// </summary>
    public required bool IsTimeLimited { get; set; }

    /// <summary>
    ///     Дата истечения доступности
    /// </summary>
    public DateTime? AvailableUntil { get; set; }

    /// <summary>
    ///     Добавлено ли через бота
    /// </summary>
    public bool IsAddedUsingBot { get; set; }
     
    /// <summary>
    ///     Данные пользователя создавшего объявление
    /// </summary>
    [NotMapped]
    public User? Creator { get; set; }

    /// <summary>
    ///     Данные пользователя обновившего объявление
    /// </summary>
    [NotMapped]
    public User? Updater { get; set; }
}