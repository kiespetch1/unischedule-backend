using UniSchedule.Abstractions.Entities;

namespace UniSchedule.Abstractions.Commands;

/// <summary>
///     Команда для удаления данных
/// </summary>
/// <typeparam name="TEntity">Тип удаляемого объекта</typeparam>
/// <typeparam name="TKey">Тип ключа сущности</typeparam>
public interface IDeleteCommand<TEntity, TKey> where TEntity : Entity<TKey>
{
    /// <summary>
    ///     Выполнить удаление
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task ExecuteAsync(TKey id, CancellationToken cancellationToken = default);
}