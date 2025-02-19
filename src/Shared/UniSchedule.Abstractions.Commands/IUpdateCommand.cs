using UniSchedule.Abstractions.Entities;

namespace UniSchedule.Abstractions.Commands;

/// <summary>
///     Команда для обновления данных
/// </summary>
/// <typeparam name="TEntity">Тип обновляемого объекта</typeparam>
/// <typeparam name="TParams">Тип параметров обновления</typeparam>
/// <typeparam name="TKey">Тип ключа сущности</typeparam>
public interface IUpdateCommand<TEntity, in TParams, TKey> where TEntity : Entity<TKey>
{
    /// <summary>
    ///     Выполнить обновление данных
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="parameters">Параметры обновления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task ExecuteAsync(TKey id, TParams parameters, CancellationToken cancellationToken = default);
}