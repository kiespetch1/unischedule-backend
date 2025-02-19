using MassTransit;

namespace UniSchedule.Abstractions.Messaging;

/// <summary>
///     Базовый класс для отправки сообщений в очередь
/// </summary>
public abstract class PublisherBase<TMessage>(IBus bus) : IPublisher<TMessage>
    where TMessage : class
{
    /// <inheritdoc />
    public async Task PublishAsync(TMessage message, CancellationToken cancellationToken = default)
    {
        await bus.Publish(message, cancellationToken);
    }
}