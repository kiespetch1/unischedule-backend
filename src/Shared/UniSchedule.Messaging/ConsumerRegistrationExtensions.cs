using MassTransit;
using UniSchedule.Messaging.Consumers.Users;

namespace UniSchedule.Messaging;

/// <summary>
///     Методы расширения для регистрации консьюмеров
/// </summary>
public static class ConsumerRegistrationExtensions
{
    /// <summary>
    ///     Добавление консьюмеров для работы с пользователями
    /// </summary>
    public static IBusRegistrationConfigurator AddUsersConsumers(
        this IBusRegistrationConfigurator configuration)
    {
        configuration.AddBatchConsumer<CreateUserConsumer, CreateUserConsumerDefinition>();
        configuration.AddBatchConsumer<UpdateUserConsumer, UpdateUserConsumerDefinition>();
        configuration.AddBatchConsumer<DeleteUserConsumer, DeleteUserConsumerDefinition>();
        configuration.AddBatchConsumer<SyncUsersConsumer, SyncUsersConsumerDefinition>();

        return configuration;
    }
}