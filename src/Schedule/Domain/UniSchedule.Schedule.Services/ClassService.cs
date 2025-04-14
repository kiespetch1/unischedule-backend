using Microsoft.EntityFrameworkCore;
using UniSchedule.Extensions.Collections;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
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

    /// <inheritdoc />
    public async Task CopyClassesToOppositeWeekAsync(
        Guid dayId,
        CancellationToken cancellationToken = default)
    {
        var sourceDay = await context.Days
            .Include(x => x.Week)
            .ThenInclude(x => x.Group)
            .ThenInclude(x => x.Weeks)
            .Include(x => x.Classes)
            .SingleOrNotFoundAsync(dayId, cancellationToken);

        var oppositeWeek = sourceDay.Week.Group.Weeks
            .SingleOrDefault(x => x.Type != sourceDay.Week.Type);

        if (oppositeWeek is null)
        {
            throw new InvalidOperationException("Противоположная неделя не найдена");
        }

        var targetDay = oppositeWeek.Days
            .SingleOrDefault(x => x.DayOfWeek == sourceDay.DayOfWeek);

        if (targetDay is null)
        {
            targetDay = new Day { DayOfWeek = sourceDay.DayOfWeek, WeekId = oppositeWeek.Id };
            context.Days.Add(targetDay);
            await context.SaveChangesAsync(cancellationToken);
        }

        foreach (var sourceClass in sourceDay.Classes)
        {
            var newClass = new Class
            {
                Name = sourceClass.Name,
                StartedAt = sourceClass.StartedAt,
                FinishedAt = sourceClass.FinishedAt,
                WeekType = sourceClass.WeekType,
                Subgroup = sourceClass.Subgroup,
                IsCancelled = sourceClass.IsCancelled,
                DayId = targetDay.Id,
                LocationId = sourceClass.LocationId,
                TeacherId = sourceClass.TeacherId
            };

            context.Classes.Add(newClass);
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}