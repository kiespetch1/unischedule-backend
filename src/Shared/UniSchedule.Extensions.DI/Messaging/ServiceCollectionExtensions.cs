using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Extensions.DI.Messaging.Settings;

namespace UniSchedule.Extensions.DI.Messaging;

/// <summary>
///     Методы расширения для DI
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Добавление конфигурации RabbitMq в DI
    /// </summary>
    /// <param name="settings">Настройки rabbitMq</param>
    /// <param name="configure">Конфигурация MassTransit</param>
    /// <param name="services">Коллекция сервисов</param>
    /// <param name="messageConfigure">Конфигурация сообщений</param>
    /// <returns>Обновленная коллекция сервисов</returns>
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, RabbitMqSettings settings,
        Action<IBusRegistrationConfigurator> configure,
        Action<IRabbitMqBusFactoryConfigurator>? messageConfigure = null)
    {
        services.AddMassTransit(x =>
        {
            configure.Invoke(x);
            x.SetSnakeCaseEndpointNameFormatter();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(settings.Host, h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });
                cfg.ConfigureEndpoints(context);
                messageConfigure?.Invoke(cfg);
            });
        });

        return services;
    }


    /// <summary>
    ///     Добавление сервиса отправки сообщений в брокер сообщений в DI
    /// </summary>
    /// <typeparam name="TPublisher">Тип сервиса отправки сообщений</typeparam>
    /// <typeparam name="TParams">Тип параметров для отправки сообщения</typeparam>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Обновленная коллекция сервисов</returns>
    public static IBusRegistrationConfigurator AddPublisher<TPublisher, TParams>(
        this IBusRegistrationConfigurator services)
        where TPublisher : class, IPublisher<TParams>
        where TParams : class
    {
        services.AddScoped<IPublisher<TParams>, TPublisher>();

        return services;
    }

    /// <summary>
    ///     Добавление пакетного обработчика сообщений
    /// </summary>
    /// <param name="configurator">Конфигуратор MassTransit</param>
    /// <typeparam name="TConsumer">Тип пакетного обработчика сообщений</typeparam>
    /// <typeparam name="TDefinition">Тип описания обработчика сообщений</typeparam>
    /// <returns>Конфигуратор MassTransit</returns>
    public static IBusRegistrationConfigurator AddBatchConsumer<TConsumer, TDefinition>(
        this IBusRegistrationConfigurator configurator)
        where TConsumer : class, IConsumer
        where TDefinition : class, IConsumerDefinition<TConsumer>
    {
        configurator.AddConsumer<TConsumer, TDefinition>(cfg =>
        {
            cfg.Options<BatchOptions>(options => options
                .SetMessageLimit(100)
                .SetTimeLimit(s: 1)
                .SetTimeLimitStart(BatchTimeLimitStart.FromLast)
                .SetConcurrencyLimit(10));
        });

        return configurator;
    }

    /// <summary>
    ///     Установка конфигурации сообщений
    /// </summary>
    /// <param name="configurator">Конфигуратор MassTransit</param>
    /// <param name="action">Настройки обработчика сообщений</param>
    public static void ConfigureMessageTopology(this IRabbitMqBusFactoryConfigurator configurator,
        Action<IRabbitMqBusFactoryConfigurator> action)
    {
        action.Invoke(configurator);
    }

    /// <summary>
    ///     Настройка публикации сообщений
    /// </summary>
    /// <param name="configurator"></param>
    /// <typeparam name="TMessage"></typeparam>
    /// <returns></returns>
    public static IRabbitMqBusFactoryConfigurator MessageConfigure<TMessage>(
        this IRabbitMqBusFactoryConfigurator configurator) where TMessage : class
    {
        configurator.Publish<TMessage>(x =>
        {
            x.ExchangeType = ExchangeType.Topic;
        });

        return configurator;
    }
}