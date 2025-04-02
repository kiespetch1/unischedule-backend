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
            if (parameters.Target.IncludedGrades.Count > 0)
            {
                query = query.Where(x =>
                    x.Target!.IncludedGrades!.Any(pt => parameters.Target.IncludedGrades.Contains(pt)));
            }

            if (parameters.Target.ExcludedGrades.Count > 0)
            {
                query = query.Where(x =>
                    x.Target!.ExcludedGrades!.Any(pt => parameters.Target.ExcludedGrades.Contains(pt)));
            }

            if (parameters.Target.IncludedGroups.Count > 0)
            {
                query = query.Where(x =>
                    x.Target!.IncludedGroups!.Any(pt => parameters.Target.IncludedGroups.Contains(pt)));
            }

            if (parameters.Target.ExcludedGroups.Count > 0)
            {
                query = query.Where(x =>
                    x.Target!.ExcludedGroups!.Any(pt => parameters.Target.ExcludedGroups.Contains(pt)));
            }

            if (parameters.Target.IncludedDepartments.Count > 0)
            {
                query = query.Where(x =>
                    x.Target!.IncludedDepartments!.Any(pt => parameters.Target.IncludedDepartments.Contains(pt)));
            }

            if (parameters.Target.ExcludedDepartments.Count > 0)
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

        return await query.ToCollectionResultAsync(cancellationToken);
    }
}