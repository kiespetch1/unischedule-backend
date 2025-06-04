using Microsoft.EntityFrameworkCore;
using UniSchedule.Abstractions.Queries;
using UniSchedule.Extensions.Collections;
using UniSchedule.Extensions.Data;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Queries;

/// <summary>
///     Запросы для работы с местами проведения
/// </summary>
public class LocationQuery(DatabaseContext context) : EFQuery<Location, Guid, LocationQueryParameters>(context)
{
    /// <inheritdoc />
    public override async Task<Location> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await BaseQuery.SingleOrNotFoundAsync(id, cancellationToken);

        return entity;
    }

    /// <inheritdoc />
    public override async Task<CollectionResult<Location>> ExecuteAsync(
        LocationQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var query = BaseQuery;

        if (!string.IsNullOrEmpty(parameters.Search))
        {
            query = query.Where(x =>
                EF.Functions.ILike(x.Name, $"%{parameters.Search}%") ||
                EF.Functions.ILike(x.Link ?? "", $"%{parameters.Search}%"));
        }

        if (parameters.LocationType is not null)
        {
            query = query.Where(x => x.Type == parameters.LocationType);
        }

        return await query.ToCollectionResultAsync(parameters, cancellationToken);
    }
}