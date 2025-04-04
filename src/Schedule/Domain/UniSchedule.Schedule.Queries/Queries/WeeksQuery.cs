using Microsoft.EntityFrameworkCore;
using UniSchedule.Abstractions.Queries;
using UniSchedule.Extensions.Collections;
using UniSchedule.Extensions.Data;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Queries.Queries;

/// <summary>
///     Запросы для работы с неделями
/// </summary>
public class WeeksQuery(DatabaseContext context) : EFQuery<Week, Guid, WeekQueryParameters>(context)
{
    /// <summary />
    private IQueryable<Week> Query => BaseQuery
        .Include(x => x.Group)
        .Include(x => x.Days)
        .ThenInclude(x => x.Classes)
        .ThenInclude(x => x.Teacher)
        .Include(x => x.Group)
        .Include(x => x.Days)
        .ThenInclude(x => x.Classes)
        .ThenInclude(x => x.Location);

    /// <inheritdoc />
    public override async Task<CollectionResult<Week>> ExecuteAsync(
        WeekQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var query = Query;

        if (parameters.GroupIds is { Count: > 0 })
        {
            query = query.Where(x => parameters.GroupIds.Contains(x.GroupId));
        }

        if (parameters.Subgroup is not null)
        {
            query = query.Where(x => x.Subgroup == parameters.Subgroup.Value);
        }

        if (parameters.Type is not null)
        {
            query = query.Where(x => x.Type == parameters.Type.Value);
        }

        return await query.ToCollectionResultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task<Week> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await Query.SingleOrNotFoundAsync(id, cancellationToken);

        return entity;
    }
}