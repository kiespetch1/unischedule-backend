using UniSchedule.Extensions.Collections;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Services.Abstractions;

namespace UniSchedule.Schedule.Services;

/// <summary>
///     Сервис для работы с парами
/// </summary>
public class ClassService(DatabaseContext context) : IClassService
{
    /// <inheritdoc />
    public async Task SetCancelledAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var @class = await context.Classes.SingleOrNotFoundAsync(id, cancellationToken);
        @class.IsCancelled = true;

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task SetActiveAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var @class = await context.Classes.SingleOrNotFoundAsync(id, cancellationToken);
        @class.IsCancelled = false;

        await context.SaveChangesAsync(cancellationToken);
    }
}