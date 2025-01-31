namespace UniSchedule.Extensions.Data;

/// <summary>
///     Выходная модель коллекции для запросов
/// </summary>
/// <typeparam name="T">Тип элементов в коллекции</typeparam>
public class CollectionResult<T>
{
    /// <summary />
    public CollectionResult()
    {
        Data = Enumerable.Empty<T>().ToList();
        TotalCount = 0;
    }

    /// <summary />
    public CollectionResult(List<T> data, int totalCount)
    {
        Data = data;
        TotalCount = totalCount;
    }

    /// <summary>
    ///     Результирующая коллекция
    /// </summary>
    public List<T> Data { get; set; }

    /// <summary>
    ///     Количество элементов в полной коллекции
    /// </summary>
    public int TotalCount { get; set; }
}