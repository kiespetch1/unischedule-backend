using Microsoft.EntityFrameworkCore;
using UniSchedule.Extensions.Collections;
using UniSchedule.Extensions.Data;
using UniSchedule.Extensions.Exceptions.Base;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Schedule.Entities.Enums;
using UniSchedule.Schedule.Services.Abstractions;
using UniSchedule.Shared.DTO.Parameters;

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

    /// <inheritdoc />
    public async Task ClearWeeksClassesAsync(Guid groupId, CancellationToken cancellationToken = default)
    {
        var group = await context.Groups
            .Include(x => x.Weeks)
            .ThenInclude(x => x.Days)
            .ThenInclude(x => x.Classes)
            .SingleOrNotFoundAsync(groupId, cancellationToken);

        group.Weeks
            .SelectMany(x => x.Days)
            .ToList()
            .ForEach(x => x.Classes.Clear());

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<CollectionResult<Class>> GetCancelledClassesAsync(
        Guid groupId,
        CancellationToken cancellationToken = default)
    {
        var classes = await context.Classes
            .AsQueryable()
            .Include(x => x.Day)
            .ThenInclude(x => x.Week)
            .Where(x => x.Day.Week.GroupId == groupId && x.IsCancelled)
            .ToCollectionResultAsync(cancellationToken);

        return classes;
    }

    /// <inheritdoc />
    public async Task CancelMultipleAsync(ClassMultipleCancelByDayIdParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var classes = await context.Classes
            .Where(x => parameters.DayIds.Contains(x.DayId))
            .ToListAsync(cancellationToken);

        foreach (var @class in classes)
        {
            @class.IsCancelled = true;
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task CancelMultipleAsync(
        ClassMultipleCancelByIdParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var classes = await context.Classes
            .Where(x => parameters.ClassIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        foreach (var @class in classes)
        {
            @class.IsCancelled = true;
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task CancelAllByWeekDaysAsync(
        ClassCancelByWeekDaysParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var evenDays = parameters.Even ?? new List<DayOfWeek>();
        var oddDays = parameters.Odd ?? new List<DayOfWeek>();

        var classesToCancel = await context.Classes
            .Include(c => c.Day)
            .ThenInclude(d => d.Week)
            .Where(c =>
                (evenDays.Count > 0 && c.Day.Week.Type == WeekType.Even && evenDays.Contains(c.Day.DayOfWeek)) ||
                (oddDays.Count > 0 && c.Day.Week.Type == WeekType.Odd && oddDays.Contains(c.Day.DayOfWeek)))
            .ToListAsync(cancellationToken);

        foreach (var @class in classesToCancel)
        {
            @class.IsCancelled = true;
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task CancelMultipleByGroupAsync(
        Guid groupId,
        CancellationToken cancellationToken = default)
    {
        var classesToCancel = await context.Classes
            .Include(c => c.Day)
            .ThenInclude(d => d.Week)
            .Where(c => c.Day.Week.GroupId == groupId && !c.IsCancelled)
            .ToListAsync(cancellationToken);

        foreach (var @class in classesToCancel)
        {
            @class.IsCancelled = true;
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}