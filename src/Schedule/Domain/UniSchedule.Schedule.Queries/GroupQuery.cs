using Microsoft.EntityFrameworkCore;
using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Abstractions.Queries;
using UniSchedule.Extensions.Collections;
using UniSchedule.Extensions.Data;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Schedule.Entities.Enums;
using UniSchedule.Schedule.Entities.Owned;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Queries;

/// <summary>
///     Запросы для работы с группами
/// </summary>
public class GroupQuery(
    DatabaseContext context,
    IUserContextProvider userContextProvider) : EFQuery<Group, Guid, GroupQueryParameters>(context)
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
        var query = parameters.FetchDetails == false ? BaseQuery : Query;

        if (parameters.Grade.HasValue)
        {
            query = query.Where(x => x.Grade == parameters.Grade);
        }

        if (parameters.HasFixedSubgroups.HasValue)
        {
            query = query.Where(x => x.HasFixedSubgroups == parameters.HasFixedSubgroups);
        }

        if (parameters.Ids is { Count: > 0 })
        {
            query = query.Where(x => parameters.Ids.Contains(x.Id));
        }

        return await query.ToCollectionResultAsync(parameters, cancellationToken);
    }

    /// <inheritdoc />
    public override async Task<Group> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var userContext = userContextProvider.GetContext();
        var group = await Query.SingleOrNotFoundAsync(id, cancellationToken);

        if (group.HasFixedSubgroups == false && userContext.IsAuthenticated)
        {
            var filteringOptions = await context.FilteringInfo
                .SingleOrDefaultAsync(x => x.CreatedBy == userContext.Id, cancellationToken);

            if (filteringOptions != null)
            {
                foreach (var week in group.Weeks)
                {
                    foreach (var day in week.Days)
                    {
                        foreach (var @class in day.Classes)
                        {
                            if (filteringOptions.ClassName == @class.Name &&
                                (@class.Subgroup != Subgroup.None ||
                                 @class.Subgroup !=
                                 filteringOptions.Subgroup))
                            {
                                @class.IsHidden = true;
                            }
                        }
                    }
                }
            }
        }

        var announcements = await context.Announcements
            .Where(a =>
                // если все еще доступно по временному ограничению
                (!a.IsTimeLimited || a.AvailableUntil > DateTime.UtcNow)
                &&
                (
                    //  есть хоть одно попадание в включение
                    a.Target.IncludedGroups.Any(x => x == group.Id) ||
                    a.Target.IncludedGrades.Any(x => x == group.Grade)
                    ||
                    //  нет совпадений ни в одном исключении
                    (a.Target.ExcludedGroups.All(x => x != group.Id) &&
                     a.Target.ExcludedGrades.All(x => x != group.Grade))
                ))
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