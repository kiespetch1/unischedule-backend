namespace UniSchedule.Extensions.Data;

/// <summary>
///     Выходная модель для запросов
/// </summary>
/// <param name="data">Результирующий объект</param>
/// <typeparam name="T">Тип объекта</typeparam>
public class Result<T>(T? data)
{
    /// <summary>
    ///     Результирующий объект
    /// </summary>
    public T? Data { get; set; } = data;

    /// <summary>
    ///     Исключение
    /// </summary>
    public Error? Error { get; set; }
}