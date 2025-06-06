using UniSchedule.Shared.DTO.Models;

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
    Task PromoteGroupsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Парсинг расписания
    /// </summary>
    /// <param name="url">Ссылка</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    public Task<List<DayParseModel>> ParseWeeksAsync(string url, CancellationToken cancellationToken = default);
}