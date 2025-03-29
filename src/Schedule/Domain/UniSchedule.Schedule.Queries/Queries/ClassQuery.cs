using UniSchedule.Abstractions.Queries;
using UniSchedule.Extensions.Collections;
using UniSchedule.Extensions.Data;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Queries.Queries;

/// <summary>
///     Запросы для работы с парами
/// </summary>
public class ClassQuery(DatabaseContext context) : EFQuery<Class, Guid, ClassQueryParameters>(context)
{
    /// <summary>
    ///     Получение пары по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор пары</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Пара</returns>
    public override async Task<Class> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await BaseQuery.SingleOrNotFoundAsync(id, cancellationToken);

        return entity;
    }

    /// <summary>
    ///     Получение списка пар
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список пар</returns>
    public override async Task<CollectionResult<Class>> ExecuteAsync(
        ClassQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var query = BaseQuery;

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

        return await query.ToCollectionResultAsync(cancellationToken);
    }
}