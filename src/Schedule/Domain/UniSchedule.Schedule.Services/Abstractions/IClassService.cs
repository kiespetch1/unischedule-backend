using UniSchedule.Extensions.Data;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Parameters;

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
    Task CancelAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Отмена статуса отмены пары
    /// </summary>
    /// <param name="id">Идентификатор пары</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task RestoreAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Восстановление нескольких пар
    /// </summary>
    /// <param name="parameters">Параметры для восстановления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task RestoreMultipleAsync(ClassMultipleRestoreParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Копирование пар дня на противоположную неделю
    /// </summary>
    /// <param name="dayId">Идентификатор дня</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task CopyClassesToOppositeWeekAsync(
        Guid dayId,
        CancellationToken cancellationToken = default);


    /// <summary>
    ///     Удаление всех пар дня
    /// </summary>
    /// <param name="dayId">Идентификатор дня</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public Task ClearDayClassesAsync(Guid dayId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Удаление всех пар группы
    /// </summary>
    /// <param name="groupId">Идентификатор группы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public Task ClearWeeksClassesAsync(Guid groupId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Получение списка всех отмененных пар группы
    /// </summary>
    /// <param name="groupId">Идентификатор группы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список отменных пар</returns>
    public Task<CollectionResult<Class>> GetCancelledClassesAsync(
        Guid groupId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Отмена нескольких пар
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public Task CancelMultipleAsync(
        ClassMultipleCancelByDayIdParameters parameters,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Отмена нескольких пар
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public Task CancelMultipleAsync(
        ClassMultipleCancelByIdParameters parameters,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Отмена пар для всех групп по дням недели
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task CancelAllByWeekDaysAsync(
        ClassCancelByWeekDaysParameters parameters,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Отмена всех пар для указанной группы
    /// </summary>
    /// <param name="groupId">Идентификатор группы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task CancelMultipleByGroupAsync(
        Guid groupId,
        CancellationToken cancellationToken = default);
}