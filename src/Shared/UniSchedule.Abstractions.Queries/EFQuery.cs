using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UniSchedule.Abstractions.Entities;
using UniSchedule.Abstractions.Queries.Base;
using UniSchedule.Extensions.Data;
using UniSchedule.Extensions.Exceptions.Base;

namespace UniSchedule.Abstractions.Queries;

/// <summary>
///     Базовый сервис запросов на получение данных с использованием EF Core
/// </summary>
/// <param name="context">Контекст базы данных</param>
/// <typeparam name="TEntity">Тип получаемых данных</typeparam>
/// <typeparam name="TKey">Тип ключа</typeparam>
/// <typeparam name="TParams">Тип параметров для получения списка данных</typeparam>
public abstract class EFQuery<TEntity, TKey, TParams>(DbContext context) :
    ISingleQuery<TEntity, TKey>,
    ISingleQueryByPredicate<TEntity, TKey>,
    IMultipleQuery<TEntity, TParams> where TEntity : Entity<TKey> where TParams : new()
{
    /// <summary>
    ///     Коллекция в БД
    /// </summary>
    protected IQueryable<TEntity> BaseQuery => context.Set<TEntity>().AsNoTracking();

    /// <inheritdoc />
    public virtual async Task<CollectionResult<TEntity>> ExecuteAsync(
        TParams parameters,
        CancellationToken cancellationToken = default)
    {
        var result = await BaseQuery.ToListAsync(cancellationToken);
        var totalCount = await BaseQuery.CountAsync(cancellationToken);

        return new CollectionResult<TEntity>(result, totalCount);
    }

    /// <inheritdoc />
    public virtual async Task<TEntity> ExecuteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await BaseQuery.SingleOrDefaultAsync(x => Equals(x.Id, id), cancellationToken);

        return entity ?? throw new NotFoundException<TEntity>(id?.ToString() ?? string.Empty);
    }

    /// <inheritdoc />
    public virtual async Task<TEntity> ExecuteAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        var entity = await BaseQuery.SingleOrDefaultAsync(predicate, cancellationToken);

        return entity ?? throw new NotFoundException($"{typeof(TEntity).Name} not found");
    }
}

/// <summary>
///     Базовый сервис запросов на получение данных с использованием EF Core
/// </summary>
/// <param name="context">Контекст базы данных</param>
/// <typeparam name="TEntity">Тип получаемых данных, содержащий идентификатор типа <see cref="Guid" /></typeparam>
/// <typeparam name="TParams">Тип параметров для получения списка данных</typeparam>
/// <remarks>
///     <see cref="Guid" /> - самый используемый тип для указания идентификаторов. Этот класс предназначен для сокращения
///     объема кода и удобства работы с сущностями, которые имеют такой идентификатор.
/// </remarks>
public abstract class EFQuery<TEntity, TParams>(DbContext context)
    : EFQuery<TEntity, Guid, TParams>(context) where TEntity : Entity<Guid> where TParams : new();