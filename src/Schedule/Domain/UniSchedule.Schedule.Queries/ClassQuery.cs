using Microsoft.EntityFrameworkCore;
using UniSchedule.Abstractions.Queries;
using UniSchedule.Extensions.Collections;
using UniSchedule.Extensions.Data;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Queries;

/// <summary>
///     Запросы для работы с парами
/// </summary>
public class ClassQuery(DatabaseContext context) : EFQuery<Class, Guid, ClassQueryParameters>(context)
{
    /// <summary />
    private IQueryable<Class> Query => context.Classes
        .Include(x => x.Day)
        .Include(x => x.Teacher)
        .Include(x => x.Location);

    /// <inheritdoc />
    public override async Task<Class> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await Query.SingleOrNotFoundAsync(id, cancellationToken);

        return entity;
    }

    /// <inheritdoc />
    public override async Task<CollectionResult<Class>> ExecuteAsync(
        ClassQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var query = Query;

        if (parameters.DayId is not null)
        {
            query = query.Where(x => parameters.DayId == x.DayId);
        }

        if (parameters.TeacherId is not null)
        {
            query = query.Where(x => parameters.TeacherId == x.TeacherId);
        }

        if (parameters.LocationId is not null)
        {
            query = query.Where(x => parameters.LocationId == x.LocationId);
        }

        if (parameters.Subgroup is not null)
        {
            query = query.Where(x => x.Subgroup == parameters.Subgroup.Value);
        }

        if (parameters.WeekType is not null)
        {
            query = query.Where(x => x.WeekType == parameters.WeekType.Value);
        }

        if (parameters.IsCancelled is not null)
        {
            query = query.Where(x => x.IsCancelled == parameters.IsCancelled.Value);
        }

        return await query.ToCollectionResultAsync(parameters, cancellationToken);
    }
}