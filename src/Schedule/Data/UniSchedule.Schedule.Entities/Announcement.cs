using System.ComponentModel.DataAnnotations.Schema;
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
    public AnnouncementTargetInfo? Target { get; set; }

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
    ///     Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///     Идентификатор создателя
    /// </summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>
    ///     Дата обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    ///     Идентификатор обновителя
    /// </summary>
    public Guid? UpdatedBy { get; set; }

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