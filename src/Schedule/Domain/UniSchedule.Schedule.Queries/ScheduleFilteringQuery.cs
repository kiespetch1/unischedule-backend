using UniSchedule.Abstractions.Queries;
using UniSchedule.Extensions.Collections;
using UniSchedule.Extensions.Data;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Queries;

/// <summary>
///     Запросы для работы с фильтрацией пользователя
/// </summary>
public class ScheduleFilteringQuery(DatabaseContext context)
    : EFQuery<ScheduleFilteringOption, Guid, ScheduleFilteringQueryParameters>(context)
{
    /// <inheritdoc />
    public override async Task<ScheduleFilteringOption> ExecuteAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await BaseQuery.SingleOrNotFoundAsync(id, cancellationToken);

        return entity;
    }

    /// <inheritdoc />
    public override async Task<CollectionResult<ScheduleFilteringOption>> ExecuteAsync(
        ScheduleFilteringQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var query = BaseQuery;
        if (parameters.UserId != null)
        {
            query = query.Where(x => x.CreatedBy == parameters.UserId.Value);
        }

        return await query.ToCollectionResultAsync(parameters, cancellationToken);
    }
}