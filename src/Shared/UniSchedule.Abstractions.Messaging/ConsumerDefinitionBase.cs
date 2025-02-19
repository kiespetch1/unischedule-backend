using MassTransit;
using RabbitMQ.Client;
using UniSchedule.Extensions.Utils;

namespace UniSchedule.Abstractions.Messaging;

/// <summary>
///     Базовый класс для описания сервиса обработки сообщений
/// </summary>
/// <typeparam name="TConsumer">Тип обработчика сообщений</typeparam>
/// <typeparam name="TMessage">Тип сообщения</typeparam>
public abstract class ConsumerDefinitionBase<TConsumer, TMessage> : ConsumerDefinition<TConsumer>
    where TConsumer : class, IConsumer
    where TMessage : class
{
    /// <summary />
    protected ConsumerDefinitionBase(string queue)
    {
        EndpointName = $"{EnvironmentUtils.ServiceName}.{queue}";
    }

    /// <inheritdoc />
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<TConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;

        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
        {
            rmq.Bind<TMessage>(x =>
            {
                x.ExchangeType = ExchangeType.Topic;
            });
        }
    }
}