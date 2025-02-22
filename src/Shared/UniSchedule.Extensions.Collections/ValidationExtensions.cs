using UniSchedule.Abstractions.Entities;
using UniSchedule.Extensions.Exceptions.Base;

namespace UniSchedule.Extensions.Collections;

/// <summary>
///     Методы расширения для валидации значений
/// </summary>
public static class ValidationExtensions
{
    /// <summary>
    ///     Выбросить исключение, если объект равен null
    /// </summary>
    /// <param name="entity">Объект</param>
    /// <param name="id">Идентификатор объекта</param>
    /// <typeparam name="TEntity">Тип объекта</typeparam>
    /// <typeparam name="TKey">Тип идентификатора объекта</typeparam>
    /// <returns>Объект</returns>
    /// <exception cref="NotFoundException">Объект равен null</exception>
    public static TEntity ThrowIfNotFound<TEntity, TKey>(this TEntity? entity, TKey id)
        where TEntity : Entity<TKey>
    {
        return entity ?? throw new NotFoundException<TEntity>(id?.ToString() ?? "unknown id");
    }

    /// <summary>
    ///     Выбросить исключение, если объект равен null
    /// </summary>
    /// <param name="entity">Объект</param>
    /// <typeparam name="TEntity">Тип объекта</typeparam>
    /// <typeparam name="TKey">Тип идентификатора объекта</typeparam>
    /// <returns>Объект</returns>
    /// <exception cref="NotFoundException{TEntity}">Объект равен null</exception>
    public static TEntity ThrowIfNotFound<TEntity, TKey>(this TEntity? entity)
        where TEntity : Entity<TKey>
    {
        return entity ?? throw new NotFoundException($"{typeof(TEntity).Name} not found");
    }

    /// <summary>
    ///     Выбросить исключение, если объект равен null
    /// </summary>
    /// <param name="entity">Объект</param>
    /// <typeparam name="TEntity">Тип объекта</typeparam>
    /// <returns>Объект</returns>
    /// <exception cref="NotFoundException{TEntity}">Объект равен null</exception>
    public static TEntity ThrowIfNotFound<TEntity>(this TEntity? entity)
        where TEntity : class
    {
        return entity ?? throw new NotFoundException($"{typeof(TEntity).Name} not found");
    }
}