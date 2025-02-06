using UniSchedule.Abstractions.Entities;
using UniSchedule.Abstractions.Entities.Auditable;
using UniSchedule.Identity.Entities.Owned;

namespace UniSchedule.Identity.Entities;

/// <summary>
///     Объявление
/// </summary>
public class Notification : Entity<Guid>, ICreatable
{
    /// <summary>
    ///     Текст уведомления
    /// </summary>
    public string TextContent { get; set; }

    /// <summary>
    ///     Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    ///     Идентификатор создателя
    /// </summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>
    ///     Информация о получателях
    /// </summary>
    public NotificationTargetInfo? Target { get; set; }
}