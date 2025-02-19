using FluentValidation;
using UniSchedule.Abstractions.Commands;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Events.Entities;
using UniSchedule.Events.Shared.Parameters;

namespace UniSchedule.Events.Commands;

/// <summary>
///     Консьюмер для создания события
/// </summary>
public class EventsConsumer(
    ICreateCommand<Event, IEnumerable<EventCreateParameters>, Guid> create,
    IValidator<EventCreateParameters> validator)
    : BatchConsumerBase<Event, EventCreateParameters, Guid>(create, validator);