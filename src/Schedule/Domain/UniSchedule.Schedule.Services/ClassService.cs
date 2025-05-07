using Microsoft.EntityFrameworkCore;
using UniSchedule.Extensions.Collections;
using UniSchedule.Extensions.Exceptions.Base;
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
            .ThenInclude(x => x.Days)
            .ThenInclude(x => x.Classes)
            .SingleOrNotFoundAsync(dayId, cancellationToken);

        var oppositeWeek = sourceDay.Week.Group.Weeks
            .SingleOrDefault(x => x.Type != sourceDay.Week.Type && x.Subgroup == sourceDay.Week.Subgroup);

        if (oppositeWeek is null)
        {
            throw new InvalidOperationException("Противоположная неделя не найдена");
        }

        var targetDay = oppositeWeek.Days
            .SingleOrDefault(x => x.DayOfWeek == sourceDay.DayOfWeek);

        if (targetDay is null)
        {
            throw new NotFoundException("Противоположный день не найден");
        }

        var targetClasses = new List<Class>();

        foreach (var sourceClass in sourceDay.Classes)
        {
            var newClass = new Class
            {
                Name = sourceClass.Name,
                StartedAt = sourceClass.StartedAt,
                FinishedAt = sourceClass.FinishedAt,
                Type = sourceClass.Type,
                WeekType = sourceClass.WeekType,
                Subgroup = sourceClass.Subgroup,
                IsCancelled = sourceClass.IsCancelled,
                LocationId = sourceClass.LocationId,
                TeacherId = sourceClass.TeacherId
            };

            context.Classes.Add(newClass);
            targetClasses.Add(newClass);
        }

        targetDay.Classes = targetClasses;

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task ClearDayClassesAsync(Guid dayId, CancellationToken cancellationToken = default)
    {
        var day = await context.Days
            .Include(x => x.Classes)
            .SingleOrNotFoundAsync(dayId, cancellationToken);

        day.Classes.Clear();
        await context.SaveChangesAsync(cancellationToken);
    }
}