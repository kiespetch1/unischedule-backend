using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Services.Abstractions;

/// <summary>
///     Интерфейс сервиса для работы с персональной фильтрацией
/// </summary>
public interface IPreferencesService
{
    /// <summary>
    ///     Устанавливает предпочтения фильтрации для пользователя
    /// </summary>
    /// <param name="parameters">Параметры фильтрации</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task SetMultipleAsync(
        ScheduleFilteringParameters parameters,
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Удаляет предпочтение фильтрации по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор предпочтения</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DeleteAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default);
}