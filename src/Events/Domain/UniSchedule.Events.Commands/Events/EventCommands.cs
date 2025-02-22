using UniSchedule.Abstractions.Commands;
using UniSchedule.Events.Database;
using UniSchedule.Events.Entities;
using UniSchedule.Events.Shared.Parameters;
using UniSchedule.Extensions.Collections;

namespace UniSchedule.Events.Commands.Events;

public class EventCommands(DatabaseContext context) :
    ICreateCommand<Event, EventCreateParameters, Guid>,
    ICreateCommand<Event, IEnumerable<EventCreateParameters>, Guid>
{
    public async Task<Guid> ExecuteAsync(
        EventCreateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var action = await context.Actions.SingleOrNotFoundAsync(parameters.ActionId, cancellationToken);
        var subject = await context.Subjects.SingleOrNotFoundAsync(parameters.SubjectId, cancellationToken);
        var entity = new Event
        {
            Action = action,
            Subject = subject,
            FieldChanges = MapChanges(parameters.FieldChanges),
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = parameters.UpdatedBy
        };

        await context.Events.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task<Guid> ExecuteAsync(
        IEnumerable<EventCreateParameters> parameters,
        CancellationToken cancellationToken = default)
    {
        var result = new List<Event>();
        foreach (var parameter in parameters)
        {
            var action = await context.Actions.SingleOrNotFoundAsync(parameter.ActionId, cancellationToken);
            var subject = await context.Subjects.SingleOrNotFoundAsync(parameter.SubjectId, cancellationToken);
            var entity = new Event
            {
                Action = action,
                Subject = subject,
                FieldChanges = MapChanges(parameter.FieldChanges),
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = parameter.UpdatedBy
            };

            result.Add(entity);
        }

        await context.Events.AddRangeAsync(result, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return result.First().Id;
    }

    /// <summary>
    ///     Преобразует список параметров создания изменений в список сущностей Change.
    /// </summary>
    /// <param name="parameters">Список параметров изменений</param>
    /// <returns>Список изменений</returns>
    private static List<Change> MapChanges(List<ChangeCreateParameters> parameters)
    {
        if (parameters == null)
        {
            return [];
        }

        return parameters.Select(param => new Change
        {
            FieldName = param.FieldName,
            OldValue = param.OldValue,
            NewValue = param.NewValue,
            IsDeleted = param.IsDeleted
        }).ToList();
    }
}