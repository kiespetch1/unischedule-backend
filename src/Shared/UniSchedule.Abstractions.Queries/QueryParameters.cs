using UniSchedule.Extensions;
using UniSchedule.Extensions.Data;

namespace UniSchedule.Abstractions.Queries;

/// <summary>
///     Базовый класс для параметров запросов на получение данных
/// </summary>
public class QueryParameters : IPageContext
{
    /// <summary />
    public QueryParameters()
    {
    }

    /// <summary>
    ///     Строка поиска
    /// </summary>
    public string? Search { get; set; }

    /// <summary>
    ///     Смещение относительно начала списка
    /// </summary>
    public int? Offset { get; set; }

    /// <summary>
    ///     Количество получаемых элементов
    /// </summary>
    public int? Limit { get; set; }

    /// <summary>
    ///     Направление сортировки
    /// </summary>
    public SortOrder? SortOrder { get; set; }

    /// <summary>
    ///     Поле сортировки
    /// </summary>
    public string? SortBy { get; set; }
}