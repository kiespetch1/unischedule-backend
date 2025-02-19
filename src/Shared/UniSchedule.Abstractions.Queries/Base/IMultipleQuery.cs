using UniSchedule.Extensions.Data;

namespace UniSchedule.Abstractions.Queries.Base;

/// <summary>
///     Базовый интерфейс для сервисов запросов на получение данных
/// </summary>
/// <typeparam name="TEntity">
///     Тип получаемых данных
/// </typeparam>
/// <typeparam name="TParams">
///     Параметры запроса на получение списка данных. Должны наследоваться от
///     <see cref="QueryParameters" />
/// </typeparam>
public interface IMultipleQuery<TEntity, in TParams> where TParams : new()
{
    /// <summary>
    ///     Асинхронное получение списка данных
    /// </summary>
    /// <param name="parameters">Параметры для получения данных</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список данных <typeparamref name="TEntity" /></returns>
    Task<CollectionResult<TEntity>> ExecuteAsync(TParams parameters, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Асинхронное получение списка данных без параметров для фильтрации
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CollectionResult<TEntity>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var parameters = new TParams();

        return ExecuteAsync(parameters, cancellationToken);
    }
}