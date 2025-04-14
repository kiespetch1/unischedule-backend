namespace UniSchedule.Schedule.Services.Abstractions;

/// <summary>
///     Интерфейс сервиса для работы с парами
/// </summary>
public interface IClassService
{
    /// <summary>
    ///     Установка статуса отмены пары
    /// </summary>
    /// <param name="id">Идентификатор пары</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task SetCancelledAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Отмена статуса отмены пары
    /// </summary>
    /// <param name="id">Идентификатор пары</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task SetActiveAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Копирование пар дня на противоположную неделю
    /// </summary>
    /// <param name="dayId">Идентификатор дня</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task CopyClassesToOppositeWeekAsync(
        Guid dayId,
        CancellationToken cancellationToken = default);
}