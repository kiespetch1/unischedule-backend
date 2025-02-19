using UniSchedule.Abstractions.Entities;

namespace UniSchedule.Abstractions.Queries.Base;

/// <summary>
///     Базовый интерфейс для сервисов запросов на получение данных по идентификатору
/// </summary>
/// <typeparam name="TEntity">
///     Тип получаемых данных
/// </typeparam>
/// <typeparam name="TKey"></typeparam>
public interface ISingleQuery<TEntity, in TKey>
    where TEntity : Entity<TKey>
{
    /// <summary>
    ///     Асинхронное получение сущности по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Сущность <typeparamref name="TEntity" /></returns>
    Task<TEntity> ExecuteAsync(TKey id, CancellationToken cancellationToken = default);
}