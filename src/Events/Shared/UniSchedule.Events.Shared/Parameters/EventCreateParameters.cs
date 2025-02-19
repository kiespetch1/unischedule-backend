namespace UniSchedule.Events.Shared.Parameters;

/// <summary>
///     Параметры создания события
/// </summary>
public class EventCreateParameters
{
    /// <summary>
    ///     Идентификатор типа совершенного действия
    /// </summary>
    public Guid ActionId { get; set; }

    /// <summary>
    ///     Список изменений полей
    /// </summary>
    public List<ChangeCreateParameters> FieldChanges { get; set; }

    /// <summary>
    ///     Идентификатор субъекта действия
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    ///     Дата обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    ///     Идентификатор пользователя, обновившего событие
    /// </summary>
    public Guid UpdatedBy { get; set; }
}