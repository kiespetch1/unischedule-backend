using Microsoft.EntityFrameworkCore;
using UniSchedule.Abstractions.Queries;
using UniSchedule.Extensions.Collections;
using UniSchedule.Extensions.Data;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Schedule.Entities.Owned;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Queries;

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

        return await query.ToCollectionResultAsync(parameters, cancellationToken);
    }

    /// <inheritdoc />
    public override async Task<Group> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var group = await Query.SingleOrNotFoundAsync(id, cancellationToken);

        var announcements = await context.Announcements
            .Where(a =>
                (!a.IsTimeLimited || a.AvailableUntil > DateTime.UtcNow)
                && (
                    a.Target == null
                    || (
                        (a.Target.IncludedGroups == null
                         || a.Target.IncludedGroups.Contains(group.Id))
                        && (a.Target.ExcludedGroups == null
                            || !a.Target.ExcludedGroups.Contains(group.Id))
                    )
                    || (
                        (a.Target.IncludedGrades == null
                         || a.Target.IncludedGrades.Contains(group.Grade))
                        && (a.Target.ExcludedGrades == null
                            || !a.Target.ExcludedGrades.Contains(group.Grade))
                    )
                )
            )
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);

        var lastAnnouncement = announcements.FirstOrDefault(a => !a.IsTimeLimited);
        var lastTimeLimitedAnnouncement = announcements.FirstOrDefault(a => a.IsTimeLimited);

        if (lastAnnouncement != null)
        {
            var creator =
                await context.Users.SingleOrDefaultAsync(x => x.Id == lastAnnouncement.CreatedBy, cancellationToken);
            lastAnnouncement.Creator = creator;
        }

        if (lastTimeLimitedAnnouncement != null)
        {
            var creator = await context.Users.SingleOrDefaultAsync(x => x.Id == lastTimeLimitedAnnouncement.CreatedBy,
                cancellationToken);
            lastTimeLimitedAnnouncement.Creator = creator;
        }

        group.AnnouncementsBlock = new AnnouncementsBlock
        {
            Last = lastAnnouncement, LastTimeLimited = lastTimeLimitedAnnouncement
        };


        return group;
    }
}