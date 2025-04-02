namespace UniSchedule.Schedule.Services.Abstractions;

/// <summary>
///     Сервис для работы с группами
/// </summary>
public interface IGroupService
{
    /// <summary>
    ///     Обновление курса для всех групп
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Задача</returns>
    Task UpdateGradesAsync(CancellationToken cancellationToken = default);
}