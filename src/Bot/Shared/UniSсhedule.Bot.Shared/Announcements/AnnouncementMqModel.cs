using UniSchedule.Abstractions.Entities;
using UniSchedule.Schedule.Entities.Enums;
using UniSchedule.Schedule.Entities.Owned;

namespace UniSсhedule.Bot.Shared.Announcements;

/// <summary>
///     Модель объявления для передачи через брокер сообщений
/// </summary>
public class AnnouncementMqModel : Entity<Guid>
{
    /// <summary>
    ///     Текст объявления
    /// </summary>
    public string Message { get; set; }

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
}