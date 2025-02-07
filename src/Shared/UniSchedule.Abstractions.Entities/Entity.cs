namespace UniSchedule.Abstractions.Entities;

/// <summary>
///     Базовый класс для всех сущностей
/// </summary>
/// <typeparam name="TKey">Тип ключа</typeparam>
public abstract class Entity<TKey>
{
    public TKey Id { get; set; }
}