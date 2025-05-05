using UniSchedule.Abstractions.Commands;
using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Extensions.Collections;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Schedule.Entities.Owned;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Commands.Commands;

/// <summary>
///     Команды для работы с объявлениями
/// </summary>
public class AnnouncementCommands(DatabaseContext context, IUserContextProvider userContextProvider) :
    ICreateCommand<Announcement, AnnouncementCreateParameters, Guid>,
    IUpdateCommand<Announcement, AnnouncementUpdateParameters, Guid>,
    IDeleteCommand<Announcement, Guid>
{
    /// <summary>
    ///     Создание объявления
    /// </summary>
    /// <param name="parameters">Параметры создания объявления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор созданного объявления</returns>
    public async Task<Guid> ExecuteAsync(
        AnnouncementCreateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var announcement = new Announcement
        {
            Message = parameters.Message,
            IsAnonymous = parameters.IsAnonymous,
            Target = new AnnouncementTargetInfo
            {
                ExcludedDepartments = parameters.Target?.ExcludedDepartments ?? [],
                ExcludedGroups = parameters.Target?.ExcludedGroups ?? [],
                ExcludedGrades = parameters.Target?.ExcludedGrades ?? [],
                IncludedDepartments = parameters.Target?.IncludedDepartments ?? [],
                IncludedGroups = parameters.Target?.IncludedGroups ?? [],
                IncludedGrades = parameters.Target?.IncludedGrades ?? []
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsTimeLimited = parameters.IsTimeLimited,
            AvailableUntil = parameters.AvailableUntil,
            CreatedBy = userContextProvider.GetContext().Id,
            UpdatedBy = userContextProvider.GetContext().Id
        };

        context.Announcements.Add(announcement);
        await context.SaveChangesAsync(cancellationToken);

        return announcement.Id;
    }

    /// <summary>
    ///     Обновление объявления
    /// </summary>
    /// <param name="id">Идентификатор объявления</param>
    /// <param name="parameters">Параметры обновления объявления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task ExecuteAsync(
        Guid id,
        AnnouncementUpdateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var announcement = await context.Announcements.SingleOrNotFoundAsync(id, cancellationToken);

        announcement.Message = parameters.Message;
        announcement.IsAnonymous = parameters.IsAnonymous;
        announcement.Target = new AnnouncementTargetInfo
        {
            ExcludedDepartments = parameters.Target?.ExcludedDepartments,
            ExcludedGroups = parameters.Target?.ExcludedGroups,
            ExcludedGrades = parameters.Target?.ExcludedGrades,
            IncludedDepartments = parameters.Target?.IncludedDepartments,
            IncludedGroups = parameters.Target?.IncludedGroups,
            IncludedGrades = parameters.Target?.IncludedGrades
        };
        announcement.IsTimeLimited = parameters.IsTimeLimited;
        announcement.AvailableUntil = parameters.AvailableUntil;
        announcement.UpdatedAt = DateTime.UtcNow;
        announcement.UpdatedBy = userContextProvider.GetContext().Id;

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    ///     Удаление объявления
    /// </summary>
    /// <param name="id">Идентификатор объявления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task ExecuteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var announcement = await context.Announcements.SingleOrNotFoundAsync(id, cancellationToken);

        context.Announcements.Remove(announcement);
        await context.SaveChangesAsync(cancellationToken);
    }
}