using UniSchedule.Abstractions.Queries;
using UniSchedule.Extensions.Collections;
using UniSchedule.Extensions.Data;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Queries.Queries;

/// <summary>
///     Запросы для работы с преподавателями
/// </summary>
public class TeacherQuery(DatabaseContext context) : EFQuery<Teacher, Guid, TeacherQueryParameters>(context)
{
    /// <summary>
    ///     Получение преподавателя по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор преподавателя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Преподаватель</returns>
    public override async Task<Teacher> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await BaseQuery.SingleOrNotFoundAsync(id, cancellationToken);

        return entity;
    }

    /// <summary>
    ///     Получение списка преподавателей
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список преподавателей</returns>
    public override async Task<CollectionResult<Teacher>> ExecuteAsync(
        TeacherQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var query = BaseQuery;

        if (!string.IsNullOrWhiteSpace(parameters.Search))
        {
            query = query.Where(x => x.FullName.Contains(parameters.Search));
        }

        return await query.ToCollectionResultAsync(cancellationToken);
    }
}