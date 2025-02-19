using System.Linq.Expressions;
using UniSchedule.Extensions.Data;

namespace UniSchedule.Abstractions.Queries.Base;

/// <summary>
///     Базовый интерфейс для сервисов запросов на получение данных по предикату
/// </summary>
/// <typeparam name="TEntity">
///     Тип получаемых данных
/// </typeparam>
/// <typeparam name="TParams">
///     Тип параметров запроса
/// </typeparam>
public interface IMultipleQueryByPredicate<TEntity, in TParams>
    where TParams : new()
{
    /// <summary>
    ///     Асинхронное получение списка данных по предикату
    /// </summary>
    /// <param name="predicate">Предикат для получения списка сущностей по предикату</param>
    /// <param name="parameters">Параметры для получения данных</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список данных <typeparamref name="TEntity" /></returns>
    Task<CollectionResult<TEntity>> ExecuteAsync(
        Expression<Func<TEntity, bool>> predicate,
        TParams parameters,
        CancellationToken cancellationToken = default);
}