using UniSchedule.Abstractions.Messaging;
using UniSchedule.Events.Shared.Parameters;

namespace UniSchedule.Events.Commands;

/// <summary>
///     Описание для обработчика сообщений
/// </summary>
public class EventsConsumerDefinition : ConsumerDefinitionBase<EventsConsumer, EventCreateParameters>
{
    /// <summary />
    public EventsConsumerDefinition() : base("events") { }
}