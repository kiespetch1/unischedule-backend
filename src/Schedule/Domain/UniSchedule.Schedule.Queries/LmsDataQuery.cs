using Microsoft.EntityFrameworkCore;
using UniSchedule.Abstractions.Queries;
using UniSchedule.Extensions.Collections;
using UniSchedule.Extensions.Data;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Queries;

/// <summary>
///     Запросы для работы с данными LMS
/// </summary>
public class LmsDataQuery(DatabaseContext context) : EFQuery<LmsData, Guid, LmsDataQueryParameters>(context)
{
    /// <summary />
    private IQueryable<LmsData> Query => BaseQuery
        .Include(x => x.Group);

    /// <inheritdoc />
    public override async Task<LmsData> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await BaseQuery
            .Include(x => x.Group)
            .SingleOrNotFoundAsync(id, cancellationToken);

        return entity;
    }

    /// <inheritdoc />
    public override async Task<CollectionResult<LmsData>> ExecuteAsync(
        LmsDataQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var query = Query;

        if (parameters.GroupId is not null)
        {
            query = query.Where(x => x.GroupId == parameters.GroupId);
        }

        if (!string.IsNullOrWhiteSpace(parameters.Search))
        {
            query = query.Where(x => EF.Functions.ILike(x.Subject, $"%{parameters.Search}%"));
        }

        return await query.ToCollectionResultAsync(parameters, cancellationToken);
    }
}