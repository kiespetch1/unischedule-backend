using UniSchedule.Schedule.Entities.Enums;
using UniSchedule.Shared.DTO.Models;

namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры создания объявления
/// </summary>
public class AnnouncementCreateParameters
{
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
}