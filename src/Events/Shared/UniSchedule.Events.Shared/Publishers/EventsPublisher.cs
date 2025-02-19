using MassTransit;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Events.Shared.Parameters;

namespace UniSchedule.Events.Shared.Publishers;

/// <summary>
///     Сервис для публикации событий в брокер сообщений
/// </summary>
public class EventsPublisher(IBus bus) : PublisherBase<EventCreateParameters>(bus);