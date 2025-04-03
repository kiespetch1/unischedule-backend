using Microsoft.EntityFrameworkCore;
using UniSchedule.Abstractions.Queries;
using UniSchedule.Extensions.Collections;
using UniSchedule.Extensions.Data;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Queries.Queries;

/// <summary>
///     Запросы для работы с объявлениями
/// </summary>
public class AnnouncementQuery(DatabaseContext context)
    : EFQuery<Announcement, Guid, AnnouncementQueryParameters>(context)
{
    /// <inheritdoc />
    public override async Task<Announcement> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await BaseQuery.SingleOrNotFoundAsync(id, cancellationToken);

        if (entity.IsAnonymous)
        {
            return entity;
        }

        if (entity.CreatedBy.HasValue)
        {
            var creator = await context.Users.SingleOrDefaultAsync(x => x.Id == entity.CreatedBy, cancellationToken);
            entity.Creator = creator;
        }

        if (entity.UpdatedBy.HasValue)
        {
            var updater = await context.Users.SingleOrDefaultAsync(x => x.Id == entity.UpdatedBy, cancellationToken);
            entity.Updater = updater;
        }

        return entity;
    }

    /// <inheritdoc />
    public override async Task<CollectionResult<Announcement>> ExecuteAsync(
        AnnouncementQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var query = BaseQuery;

        if (parameters.Target is not null)
        {
            if (parameters.Target.IncludedGrades is { Count: > 0 })
            {
                query = query.Where(x =>
                    x.Target!.IncludedGrades!.Any(pt => parameters.Target.IncludedGrades.Contains(pt)));
            }

            if (parameters.Target.ExcludedGrades is { Count: > 0 })
            {
                query = query.Where(x =>
                    x.Target!.ExcludedGrades!.Any(pt => parameters.Target.ExcludedGrades.Contains(pt)));
            }

            if (parameters.Target.IncludedGroups is { Count: > 0 })
            {
                query = query.Where(x =>
                    x.Target!.IncludedGroups!.Any(pt => parameters.Target.IncludedGroups.Contains(pt)));
            }

            if (parameters.Target.ExcludedGroups is { Count: > 0 })
            {
                query = query.Where(x =>
                    x.Target!.ExcludedGroups!.Any(pt => parameters.Target.ExcludedGroups.Contains(pt)));
            }

            if (parameters.Target.IncludedDepartments is { Count: > 0 })
            {
                query = query.Where(x =>
                    x.Target!.IncludedDepartments!.Any(pt => parameters.Target.IncludedDepartments.Contains(pt)));
            }

            if (parameters.Target.ExcludedDepartments is { Count: > 0 })
            {
                query = query.Where(x =>
                    x.Target!.ExcludedDepartments!.Any(pt => parameters.Target.ExcludedDepartments.Contains(pt)));
            }
        }

        if (!string.IsNullOrEmpty(parameters.Search))
        {
            query = query.Where(x =>
                EF.Functions.ILike(x.Message, $"%{parameters.Search}%"));
        }

        if (parameters.CreatedBy is not null)
        {
            query = query.Where(x => parameters.CreatedBy == x.CreatedBy);
        }

        if (parameters.IsAnonymous is not null)
        {
            query = query.Where(x => x.IsAnonymous == parameters.IsAnonymous.Value);
        }

        var result = await query.ToCollectionResultAsync(cancellationToken);

        if (result.Data.Any(x => x.IsAnonymous == false))
        {
            return await GetUserInfoAsync(result, cancellationToken)!;
        }

        return result;
    }

    private async Task<CollectionResult<Announcement>> GetUserInfoAsync(
        CollectionResult<Announcement> result,
        CancellationToken cancellationToken = default)
    {
        var announcements = result.Data.Where(x => x.IsAnonymous == false).ToList();
        var createdByIds = announcements
            .Select(c => c.CreatedBy)
            .Where(id => id.HasValue)
            .Select(id => id!.Value)
            .Distinct()
            .ToList();
        var updatedByIds = announcements
            .Select(c => c.UpdatedBy)
            .Where(id => id.HasValue)
            .Select(id => id!.Value)
            .Distinct()
            .ToList();

        if (createdByIds.Count != 0)
        {
            var users = await context.Users
                .Where(u => createdByIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, cancellationToken);

            foreach (var announcement in announcements)
            {
                if (announcement is { IsAnonymous: false, CreatedBy: not null } &&
                    users.TryGetValue(announcement.CreatedBy.Value, out var user))
                {
                    announcement.Creator = user;
                }
            }
        }

        if (updatedByIds.Count != 0)
        {
            var users = await context.Users
                .Where(u => updatedByIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, cancellationToken);

            foreach (var announcement in announcements)
            {
                if (announcement is { IsAnonymous: false, UpdatedBy: not null } &&
                    users.TryGetValue(announcement.UpdatedBy.Value, out var user))
                {
                    announcement.Updater = user;
                }
            }
        }

        return result;
    }
}