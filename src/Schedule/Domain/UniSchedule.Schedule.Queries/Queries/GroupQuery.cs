using Microsoft.EntityFrameworkCore;
using UniSchedule.Abstractions.Queries;
using UniSchedule.Extensions.Collections;
using UniSchedule.Extensions.Data;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Schedule.Entities.Owned;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Queries.Queries;

/// <summary>
///     Запросы для работы с группами
/// </summary>
public class GroupQuery(DatabaseContext context) : EFQuery<Group, Guid, GroupQueryParameters>(context)
{
    /// <summary />
    private IQueryable<Group> Query => BaseQuery
        .Include(x => x.Weeks)
        .ThenInclude(x => x.Days)
        .ThenInclude(x => x.Classes)
        .ThenInclude(x => x.Location)
        .Include(x => x.Weeks)
        .ThenInclude(x => x.Days.OrderBy(d => d.DayOfWeek))
        .ThenInclude(x => x.Classes)
        .ThenInclude(x => x.Teacher);

    /// <inheritdoc />
    public override async Task<CollectionResult<Group>> ExecuteAsync(
        GroupQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var query = Query;

        if (parameters.Grade.HasValue)
        {
            query = query.Where(x => x.Grade == parameters.Grade);
        }

        if (parameters.HasSubgroups.HasValue)
        {
            query = query.Where(x => x.HasSubgroups == parameters.HasSubgroups);
        }

        if (parameters.HasFixedSubgroups.HasValue)
        {
            query = query.Where(x => x.HasFixedSubgroups == parameters.HasFixedSubgroups);
        }

        return await query.ToCollectionResultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task<Group> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await Query.SingleOrNotFoundAsync(id, cancellationToken);

        var announcements = context.Announcements
            .Where(x =>
                x.Target != null &&
                (x.IsTimeLimited == false || x.AvailableUntil > DateTime.UtcNow) &&
                ((x.Target.IncludedGroups != null && x.Target.IncludedGroups.Contains(entity.Id) &&
                  x.Target.ExcludedGroups != null && !x.Target.ExcludedGroups.Contains(entity.Id)) ||
                 (x.Target.IncludedGrades != null && x.Target.IncludedGrades.Contains(entity.Grade) &&
                  x.Target.ExcludedGrades != null && !x.Target.ExcludedGrades.Contains(entity.Grade)))
            )
            .OrderByDescending(x => x.CreatedAt);

        var result = await announcements
            .Select(x => new { All = x, TimeLimited = x.IsTimeLimited ? x : null })
            .ToListAsync(cancellationToken);

        var lastAnnouncement = result.FirstOrDefault(r => r.TimeLimited == null)?.All;
        var lastLimitedTimeAnnouncement = result
            .Where(r => r.TimeLimited != null)
            .Select(r => r.TimeLimited)
            .FirstOrDefault();

        var announcementsBlock =
            new AnnouncementsBlock { Last = lastAnnouncement, LastTimeLimited = lastLimitedTimeAnnouncement };
        entity.AnnouncementsBlock = announcementsBlock;

        return entity;
    }
}