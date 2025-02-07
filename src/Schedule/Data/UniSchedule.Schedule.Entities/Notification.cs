using UniSchedule.Abstractions.Entities;
using UniSchedule.Abstractions.Entities.Auditable;
using UniSchedule.Schedule.Entities.Owned;

namespace UniSchedule.Schedule.Entities;

/// <summary>
///     Объявление
/// </summary>
public class Notification : Entity<Guid>, ICreatable
{
    /// <summary>
    ///     Текст уведомления
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    ///     Информация о получателях
    /// </summary>
    public NotificationTargetInfo? Target { get; set; }

    /// <summary>
    ///     Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///     Идентификатор создателя
    /// </summary>
    public Guid? CreatedBy { get; set; }
}