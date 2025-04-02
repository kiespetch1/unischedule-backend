using Microsoft.EntityFrameworkCore;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Services.Abstractions;

namespace UniSchedule.Schedule.Services;

/// <summary>
///     Сервис для работы с группами
/// </summary>
public class GroupService(DatabaseContext context) : IGroupService
{
    /// <inheritdoc />
    public async Task UpdateGradesAsync(CancellationToken cancellationToken = default)
    {
        var groups = await context.Groups
            .Where(x => x.Grade < 4)
            .ToListAsync(cancellationToken);

        foreach (var group in groups)
        {
            group.Grade++;
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}