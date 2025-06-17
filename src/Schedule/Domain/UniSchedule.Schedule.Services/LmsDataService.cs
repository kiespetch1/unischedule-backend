using UniSchedule.Extensions.Collections;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Schedule.Services.Abstractions;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Services;

/// <summary>
///     Сервис для работы с данными LMS
/// </summary>
public class LmsDataService(DatabaseContext context) : ILmsDataService
{
    /// <inheritdoc />
    public async Task<Guid> CreateAsync(LmsDataCreateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var entity = new LmsData { Subject = parameters.Subject, Data = parameters.Data, GroupId = parameters.GroupId };

        context.LmsData.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Guid id, LmsDataUpdateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var entity = await context.LmsData.SingleOrNotFoundAsync(id, cancellationToken);

        entity.Subject = parameters.Subject;
        entity.Data = parameters.Data;
        entity.GroupId = parameters.GroupId;

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await context.LmsData.SingleOrNotFoundAsync(id, cancellationToken);

        context.LmsData.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }
}