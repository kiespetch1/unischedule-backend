using System.Linq.Expressions;
using UniSchedule.Abstractions.Entities;

namespace UniSchedule.Abstractions.Queries.Base;

/// <summary>
///     Базовый интерфейс для сервисов запросов на получение данных по предикату
/// </summary>
/// <typeparam name="TEntity">
///     Тип получаемых данных
/// </typeparam>
/// <typeparam name="TKey">
///     Тип ключа сущности
/// </typeparam>
public interface ISingleQueryByPredicate<TEntity, TKey>
    where TEntity : Entity<TKey>
{
    /// <summary>
    ///     Асинхронное получение сущности по предикату
    /// </summary>
    /// <param name="predicate">Предикат для поиска объекта</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Запрашиваемый объект</returns>
    Task<TEntity> ExecuteAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);
}