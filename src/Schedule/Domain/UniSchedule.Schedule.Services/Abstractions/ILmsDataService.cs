using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Services.Abstractions;

/// <summary>
///     Интерфейс сервиса для работы с данными LMS
/// </summary>
public interface ILmsDataService
{
    /// <summary>
    ///     Создание данных LMS
    /// </summary>
    Task<Guid> CreateAsync(LmsDataCreateParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Обновление данных LMS
    /// </summary>
    Task UpdateAsync(Guid id, LmsDataUpdateParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Удаление данных LMS
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}