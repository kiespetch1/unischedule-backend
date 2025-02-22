using System.Linq.Expressions;
using UniSchedule.Abstractions.Entities;

namespace UniSchedule.Validation;

/// <summary>
///     Валидатор для сущностей
/// </summary>
/// <typeparam name="TEntity">Тип сущности</typeparam>
/// <typeparam name="TKey">Тип ключа сущности</typeparam>
public interface IEntityValidator<TEntity, TKey> where TEntity : Entity<TKey>
{
    /// <summary>
    ///     Проверка на существование сущности
    /// </summary>
    /// <param name="expression">Предикат с условием выборки</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task ThrowIfNotExistsAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Проверка на существование сущности по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task ThrowIfNotExistsAsync(TKey id, CancellationToken cancellationToken = default);
}