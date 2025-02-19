using UniSchedule.Abstractions.Entities;

namespace UniSchedule.Abstractions.Commands;

/// <summary>
///     Команда для создания данных
/// </summary>
/// <typeparam name="TEntity">Тип создаваемого объекта</typeparam>
/// <typeparam name="TParams">Тип параметров создания</typeparam>
/// <typeparam name="TKey">Тип ключа сущности</typeparam>
public interface ICreateCommand<TEntity, in TParams, TKey> where TEntity : Entity<TKey>
{
    /// <summary>
    ///     Выполнить создание данных
    /// </summary>
    /// <param name="parameters">Параметры создания</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task<TKey> ExecuteAsync(TParams parameters, CancellationToken cancellationToken = default);
}