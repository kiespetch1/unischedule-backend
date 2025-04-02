using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UniSchedule.Abstractions.Entities;
using UniSchedule.Extensions.Data;
using UniSchedule.Extensions.Exceptions.Base;

namespace UniSchedule.Extensions.Collections;

/// <summary>
/// </summary>
public static partial class QueryableExtensions
{
    /// <summary>
    ///     Представление <see cref="IQueryable{T}" /> в <see cref="query" />
    /// </summary>
    /// <param name="query">Коллекция</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <returns>Коллекция, обёрнутая в <see cref="CollectionResult{T}" /></returns>
    public static async Task<CollectionResult<T>> ToCollectionResultAsync<T>(
        this IQueryable<T> query,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);
        var data = await query.ToListAsync(cancellationToken);

        return new CollectionResult<T>(data, totalCount);
    }

    /// <summary>
    ///     Представление <see cref="IQueryable{T}" /> в <see cref="CollectionResult{T}" />
    /// </summary>
    /// <param name="query">Коллекция</param>
    /// <param name="pageContext">Контекст для постраничного вывода</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <returns>Коллекция, обёрнутая в <see cref="CollectionResult{T}" /></returns>
    public static async Task<CollectionResult<T>> ToCollectionResultAsync<T>(
        this IQueryable<T> query,
        IPageContext pageContext,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);

        query = query.OrderBy(pageContext.SortBy, pageContext.SortOrder);

        if (pageContext.Offset.HasValue)
        {
            query = query.Skip(pageContext.Offset.Value);
        }

        if (pageContext.Limit.HasValue)
        {
            query = query.Take(pageContext.Limit.Value);
        }

        var data = await query.ToListAsync(cancellationToken);

        return new CollectionResult<T>(data, totalCount);
    }

    /// <summary>
    ///     Получение единственного элемента из коллекции по идентификатору с типом <see cref="Guid" />.
    ///     Если элемент не найден, то будет выброшено исключение<see cref="query" />.
    /// </summary>
    /// <param name="query">Коллекция <see cref="id" />"/></param>
    /// <param name="id">Идентификатор</param>
    /// <typeparam name="TEntity">Тип элементов коллекции</typeparam>
    /// <typeparam name="TKey">Тип ключа сущности</typeparam>
    /// <returns>Элемент коллекции</returns>
    public static TEntity SingleOrNotFound<TEntity, TKey>(this IQueryable<TEntity> query, TKey id)
        where TEntity : Entity<TKey>
    {
        return query.SingleOrDefault(e => e.Id!.Equals(id)).ThrowIfNotFound(id);
    }

    /// <summary>
    ///     Получение единственного элемента из коллекции по идентификатору с типом <see cref="Guid" />.
    ///     Если элемент не найден, то будет выброшено исключение <see cref="NotFoundException{TEntity}" />.
    /// </summary>
    /// <param name="query">Коллекция <see cref="IQueryable{T}" /></param>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <typeparam name="TEntity">Тип элементов в коллекции</typeparam>
    /// <typeparam name="TKey">Тип ключа сущности</typeparam>
    /// <returns>Элемент коллекции</returns>
    public static async Task<TEntity> SingleOrNotFoundAsync<TEntity, TKey>(
        this IQueryable<TEntity> query,
        TKey id,
        CancellationToken cancellationToken = default)
        where TEntity : Entity<TKey>
    {
        return (await query.SingleOrDefaultAsync(e => e.Id!.Equals(id), cancellationToken)).ThrowIfNotFound(id);
    }

    /// <summary>
    ///     Получение единственного элемента из коллекции по предикату.
    ///     Если элемент не найден, то будет выброшено исключение <see cref="NotFoundException{TEntity}" />.
    /// </summary>
    /// <param name="query">Коллекция <see cref="IQueryable{T}" /></param>
    /// <param name="predicate">Логический предикат</param>
    /// <typeparam name="TEntity">Тип элементов в коллекции</typeparam>
    /// <returns>Элемент коллекции</returns>
    public static TEntity SingleOrNotFound<TEntity>(
        this IQueryable<TEntity> query, Expression<Func<TEntity, bool>> predicate)
        where TEntity : class
    {
        return query.SingleOrDefault(predicate).ThrowIfNotFound();
    }

    /// <summary>
    ///     Получение единственного элемента из коллекции по предикату.
    ///     Если элемент не найден, то будет выброшено исключение <see cref="NotFoundException{TEntity}" />.
    /// </summary>
    /// <param name="query">Коллекция <see cref="IQueryable{T}" /></param>
    /// <param name="predicate">Логический предикат</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <typeparam name="TEntity">Тип элементов в коллекции</typeparam>
    /// <returns>Элемент коллекции</returns>
    public static async Task<TEntity> SingleOrNotFoundAsync<TEntity>(
        this IQueryable<TEntity> query,
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        return (await query.SingleOrDefaultAsync(predicate, cancellationToken)).ThrowIfNotFound();
    }

    /// <summary>
    ///     Операция Include для объектов с сильной вложенностью
    /// </summary>
    /// <param name="source">Коллекция <see cref="IQueryable{T}" /></param>
    /// <param name="navigationPropertyPath">Объекты верхнего уровня</param>
    /// <param name="nextProperties">Объекты нижнего уровня</param>
    /// <typeparam name="TEntity">Тип элементов в коллекции</typeparam>
    /// <typeparam name="TProperty">Вложенный объект, по которому делается Include</typeparam>
    /// <returns>Коллекция с вложенными данными</returns>
    public static IQueryable<TEntity> MultipleThenInclude<TEntity, TProperty>(
        this IQueryable<TEntity> source,
        Expression<Func<TEntity, IEnumerable<TProperty>>> navigationPropertyPath,
        params Expression<Func<TProperty, object>>[] nextProperties)
        where TEntity : class
    {
        return nextProperties.Aggregate(source, (current, nextProperty) =>
            current
                .Include(navigationPropertyPath)
                .ThenInclude(nextProperty));
    }

    /// <summary>
    ///     Операция Include для объектов с сильной вложенностью
    /// </summary>
    /// <param name="source">Коллекция <see cref="IQueryable{T}" /></param>
    /// <param name="navigationPropertyPath">Объекты верхнего уровня</param>
    /// <param name="nextProperties">Объекты нижнего уровня</param>
    /// <typeparam name="TEntity">Тип элементов в коллекции</typeparam>
    /// <typeparam name="TProperty">Вложенный объект, по которому делается Include</typeparam>
    /// <returns>Коллекция с вложенными данными</returns>
    public static IQueryable<TEntity> MultipleThenInclude<TEntity, TProperty>(
        this IQueryable<TEntity> source,
        Expression<Func<TEntity, TProperty>> navigationPropertyPath,
        params Expression<Func<TProperty, object>>[] nextProperties)
        where TEntity : class
    {
        return nextProperties.Aggregate(source, (current, nextProperty) =>
            current
                .Include(navigationPropertyPath)
                .ThenInclude(nextProperty));
    }

    /// <summary>
    ///     Проверка на существование сущности по ее идентификатору
    /// </summary>
    /// <param name="source">Коллекция <see cref="IQueryable{T}" /></param>
    /// <param name="id">Идентификатор сущности</param>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <typeparam name="TKey">Тип идентификатора</typeparam>
    /// <returns></returns>
    public static bool IsExist<TEntity, TKey>(this IQueryable<TEntity> source, TKey id)
        where TEntity : Entity<TKey>
    {
        return source.AsNoTracking().Any(e => e.Id!.Equals(id));
    }

    /// <summary>
    ///     Проверка на существование списка сущностей по их идентификаторам
    /// </summary>
    /// <param name="source">Коллекция <see cref="IQueryable{T}" /></param>
    /// <param name="ids">Список идентификаторов сущностей</param>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <typeparam name="TKey">Тип идентификатора</typeparam>
    public static bool IsExists<TEntity, TKey>(this IQueryable<TEntity> source, List<TKey> ids)
        where TEntity : Entity<TKey>
    {
        var entityIds = source
            .Where(e => ids.Contains(e.Id))
            .Select(e => e.Id)
            .ToList();

        var excepted = ids.Except(entityIds);

        return !excepted.Any();
    }
}