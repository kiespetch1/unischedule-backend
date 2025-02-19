namespace UniSchedule.Events.Shared.Parameters;

/// <summary>
///     Параметры создания записи изменения
/// </summary>
public class ChangeCreateParameters
{
    /// <summary>
    ///     Имя измененного поля
    /// </summary>
    public string FieldName { get; set; }

    /// <summary>
    ///     Старое значение поля
    /// </summary>
    public string? OldValue { get; set; }

    /// <summary>
    ///     Новое значение поля
    /// </summary>
    public string NewValue { get; set; }

    /// <summary>
    ///     Является ли изменение удалением
    /// </summary>
    public bool IsDeleted { get; set; }
}