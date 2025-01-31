using R.Tech.Extensions;

namespace UniSchedule.Extensions.Data;

/// <summary>
///     Контекст для постраничного вывода
/// </summary>
public interface IPageContext
{
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