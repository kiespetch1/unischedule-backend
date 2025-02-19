using UniSchedule.Abstractions.Entities;

namespace UniSchedule.Events.Entities;

public class Event : Entity<Guid>
{
    /// <summary>
    ///     Тип совершенного действия
    /// </summary>
    public required Action Action { get; set; }

    /// <summary>
    ///     Список изменений полей
    /// </summary>
    public List<Change> FieldChanges { get; set; }

    /// <summary>
    ///     Субъект действия
    /// </summary>
    public required Subject Subject { get; set; }

    /// <summary>
    ///     Дата обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    ///     Идентификатор пользователя, обновившего событие
    /// </summary>
    public Guid UpdatedBy { get; set; }
}