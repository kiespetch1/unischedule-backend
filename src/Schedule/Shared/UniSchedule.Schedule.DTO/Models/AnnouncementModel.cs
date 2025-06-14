﻿using UniSchedule.Schedule.Entities.Enums;

namespace UniSchedule.Shared.DTO.Models;

/// <summary>
///     Модель "Объявление"
/// </summary>
public class AnnouncementModel
{
    /// <summary>
    ///     Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Текст объявления
    /// </summary>
    public required string Message { get; set; }

    /// <summary>
    ///     Информация о получателях
    /// </summary>
    public AnnouncementTargetModel? Target { get; set; }

    /// <summary>
    ///     Приоритет
    /// </summary>
    public AnnouncementPriority Priority { get; set; }

    /// <summary>
    ///     Является ли анонимным
    /// </summary>
    public bool IsAnonymous { get; set; }

    /// <summary>
    ///     Является ли доступным ограниченное время
    /// </summary>
    public bool IsTimeLimited { get; set; }

    /// <summary>
    ///     Дата истечения доступности
    /// </summary>
    public DateTime? AvailableUntil { get; set; }

    /// <summary>
    ///     Добавлено ли через бота
    /// </summary>
    public bool IsAddedUsingBot { get; set; }

    /// <summary>
    ///     Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///    Данные о создателе
    /// </summary>
    public UserModel? CreatedBy { get; set; }

    /// <summary>
    ///     Дата обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    ///     Данные об обновителе
    /// </summary>
    public UserModel? UpdatedBy { get; set; }
}