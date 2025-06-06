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
    ///     Импорт расписания пар для группы с официального сайта
    /// </summary>
    /// <param name="groupId">Идентификатор группы</param>
    /// <param name="url">Ссылка на страницу расписания</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task ImportClassesScheduleAsync(Guid groupId, string url, CancellationToken cancellationToken = default);
}