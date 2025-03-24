using UniSchedule.Abstractions.Queries;
using UniSchedule.Extensions.Collections;
using UniSchedule.Extensions.Data;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Queries;

/// <summary>
///     Запросы для работы с неделями
/// </summary>
public class WeeksQuery(DatabaseContext context) : EFQuery<Week, Guid, WeekQueryParameters>(context)
{
    /// <summary>
    ///     Получение списка недель
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public override async Task<CollectionResult<Week>> ExecuteAsync(
        WeekQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var query = BaseQuery;

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
            query = query.Where(x => x.WeekType == parameters.Type.Value);
        }

        return await query.ToCollectionResultAsync(cancellationToken);
    }

    public override async Task<Week> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await BaseQuery.SingleOrNotFoundAsync(id, cancellationToken);

        return entity;
    }
}