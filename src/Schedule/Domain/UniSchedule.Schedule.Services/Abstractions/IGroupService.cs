using UniSchedule.Shared.DTO.Parameters;

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
    /// <param name="parameters">Параметры импорта расписания</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task ImportClassesScheduleAsync(ClassScheduleImportParameters parameters,
        CancellationToken cancellationToken = default);
}