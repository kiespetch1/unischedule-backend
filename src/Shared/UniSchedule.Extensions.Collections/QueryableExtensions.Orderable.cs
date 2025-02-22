using System.Linq.Expressions;

namespace UniSchedule.Extensions.Collections;

/// <summary>
///     Методы расширения для <see cref="IQueryable" />
/// </summary>
public static partial class QueryableExtensions
{
    /// <summary>
    ///     Сортировка выборки
    /// </summary>
    /// <param name="query">Запрос</param>
    /// <param name="path">Поле по которому осуществляется сортировка</param>
    /// <param name="order">Порядок сортировки</param>
    /// <typeparam name="TProperty">Тип значения по которому происходит сортировка</typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public static IQueryable<TEntity> OrderBy<TEntity, TProperty>(
        this IQueryable<TEntity> query,
        Expression<Func<TEntity, TProperty>> path,
        SortOrder order)
    {
        return order == SortOrder.Ascending
            ? query.OrderBy(path)
            : query.OrderByDescending(path);
    }

    /// <summary>
    ///     Сортировка по возрастанию
    /// </summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="query">Коллекция</param>
    /// <param name="propertyName">Наименование (ключ) свойства, по которому производится сортировка</param>
    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string? propertyName)
    {
        return CallOrderedQueryable(query, "OrderBy", propertyName);
    }

    /// <summary>
    ///     Сортировка по убыванию
    /// </summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="query">Коллекция</param>
    /// <param name="propertyName">Наименование (ключ) свойства, по которому производится сортировка</param>
    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string? propertyName)
    {
        return CallOrderedQueryable(query, "OrderByDescending", propertyName);
    }

    /// <summary>
    ///     Сортировка по возрастанию
    /// </summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="query">Коллекция</param>
    /// <param name="propertyName">Наименование (ключ) свойства, по которому производится сортировка</param>
    public static IOrderedQueryable<T> ThenBy<T>(this IQueryable<T> query, string? propertyName)
    {
        return CallOrderedQueryable(query, "ThenBy", propertyName);
    }

    /// <summary>
    ///     Сортировка по убыванию
    /// </summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="query">Коллекция</param>
    /// <param name="propertyName">Наименование (ключ) свойства, по которому производится сортировка</param>
    public static IOrderedQueryable<T> ThenByDescending<T>(this IQueryable<T> query, string? propertyName)
    {
        return CallOrderedQueryable(query, "ThenByDescending", propertyName);
    }

    /// <summary>
    ///     Сортировка
    /// </summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="query">Коллекция</param>
    /// <param name="propertyName">Наименование (ключ) свойства, по которому производится сортировка</param>
    /// <param name="order">Порядок сортировки</param>
    /// s>
    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string? propertyName, SortOrder? order)
    {
        order ??= SortOrder.Ascending;

        return order == SortOrder.Ascending
            ? query.OrderBy(propertyName)
            : query.OrderByDescending(propertyName);
    }

    /// <summary>
    ///     Сортировка
    /// </summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    /// <param name="source">Коллекция</param>
    /// <param name="propertyName">Наименование (ключ) свойства, по которому производится сортировка</param>
    /// <param name="order">Порядок сортировки</param>
    /// s>
    public static IOrderedQueryable<T> ThenBy<T>(this IQueryable<T> source, string? propertyName, SortOrder order)
    {
        if (order == SortOrder.Ascending)
        {
            return source.ThenBy(propertyName);
        }

        return source.ThenByDescending(propertyName);
    }

    /// <summary>
    ///     Генерация вызова методов Queryable с использованием наименования свойства
    /// </summary>
    private static IOrderedQueryable<T> CallOrderedQueryable<T>(this IQueryable<T> query, string methodName,
        string? propertyName)
    {
        propertyName ??= typeof(T).GetProperties().First().Name;

        var param = Expression.Parameter(typeof(T), "a");
        var body = propertyName.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);

        return (IOrderedQueryable<T>)query.Provider.CreateQuery(
            Expression.Call(
                typeof(Queryable),
                methodName,
                new[] { typeof(T), body.Type },
                query.Expression,
                Expression.Lambda(body, param)
            )
        );
    }
}