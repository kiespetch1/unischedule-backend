using System.Collections;
using Humanizer;
using UniSchedule.Extensions.Data;

namespace UniSchedule.Extensions.Collections;

/// <summary>
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    ///     Представление <see cref="IEnumerable{T}" /> в <see cref="CollectionResult{T}" />
    /// </summary>
    /// <param name="source">Коллекция</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <returns>Коллекция, обёрнутая в <see cref="CollectionResult{T}" /></returns>
    public static async Task<CollectionResult<T>> ToCollectionResultAsync<T>(
        this IEnumerable<T> source,
        CancellationToken cancellationToken = default)
    {
        return await source.ToCollectionResultAsync(null, cancellationToken);
    }

    /// <summary>
    ///     Представление <see cref="IEnumerable{T}" /> в <see cref="CollectionResult{T}" />
    /// </summary>
    /// <param name="source">Коллекция</param>
    /// <param name="pageContext">Контекст для постраничного вывода</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <returns>Коллекция, обёрнутая в <see cref="CollectionResult{T}" /></returns>
    public static async Task<CollectionResult<T>> ToCollectionResultAsync<T>(
        this IEnumerable<T> source,
        IPageContext? pageContext,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await Task.Run(() => source.Count(), cancellationToken);

        if (!string.IsNullOrEmpty(pageContext?.SortBy))
        {
            source = pageContext.SortOrder == SortOrder.Ascending
                ? source.OrderBy(e => GetPropertyValue(e, pageContext.SortBy))
                : source.OrderByDescending(e => GetPropertyValue(e, pageContext.SortBy));
        }

        if (pageContext?.Offset.HasValue == true)
        {
            source = source.Skip(pageContext.Offset.Value);
        }

        if (pageContext?.Limit.HasValue == true)
        {
            source = source.Take(pageContext.Limit.Value);
        }

        var data = await Task.Run(() => source.ToList(), cancellationToken);

        return new CollectionResult<T>(data, totalCount);
    }

    /// <summary>
    ///     Проверяет, является ли содержимое частичного набора подмножеством содержимого полного набора
    /// </summary>
    /// <typeparam name="T">Тип элементов коллекций. Должен поддерживать сравнение на равенство</typeparam>
    /// <param name="partial">Частичный набор элементов</param>
    /// <param name="full">Полный набор элементов</param>
    /// <returns>
    ///     Значение true, если перечисление partial является подмножеством full;
    ///     в противном случае — значение false.
    /// </returns>
    public static bool IsSubsetOf<T>(this IEnumerable<T> partial, IEnumerable<T> full)
    {
        if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(IDictionary))
        {
            throw new InvalidOperationException("Этот метод не поддерживает словари.");
        }

        ArgumentNullException.ThrowIfNull(partial);
        ArgumentNullException.ThrowIfNull(full);

        var fullSet = new HashSet<T>(full);
        return partial.All(item => fullSet.Contains(item));
    }

    private static object GetPropertyValue<T>(T item, string propertyName)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        if (propertyName == null)
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        var propertyInfo = typeof(T).GetProperty(propertyName.Dehumanize());
        if (propertyInfo == null)
        {
            throw new ArgumentException($"Property {propertyName} not found on type {typeof(T).FullName}");
        }

        return propertyInfo.GetValue(item, null)!;
    }
}