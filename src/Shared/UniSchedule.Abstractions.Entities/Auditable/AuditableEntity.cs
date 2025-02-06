﻿namespace UniSchedule.Abstractions.Entities.Auditable;

/// <summary>
///     Базовый класс для сущностей с отслеживанием изменений
/// </summary>
public abstract class AuditableEntity<TKey> :
    Entity<TKey>,
    ICreatable,
    IUpdatable,
    IDeletable
{
    /// <inheritdoc />
    public DateTime CreatedAt { get; set; }

    /// <inheritdoc />
    public Guid? CreatedBy { get; set; }

    /// <inheritdoc />
    public DateTime? DeletedAt { get; set; }

    /// <inheritdoc />
    public Guid? DeletedBy { get; set; }

    /// <inheritdoc />
    public DateTime UpdatedAt { get; set; }

    /// <inheritdoc />
    public Guid? UpdatedBy { get; set; }
}