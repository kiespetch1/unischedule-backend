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
}