using UniSchedule.Abstractions.Entities;

namespace UniSchedule.Events.Entities;

/// <summary>
///     Информация об изменении сущности
/// </summary>
public class Change : Entity<Guid>
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