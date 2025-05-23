﻿using MassTransit;
using UniSchedule.Messaging.Consumers.Announcements;
using UniSchedule.Messaging.Consumers.Groups;
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

    /// <summary>
    ///     Добавление консьюмеров для работы с группами
    /// </summary>
    public static IBusRegistrationConfigurator AddGroupsConsumers(
        this IBusRegistrationConfigurator configuration)
    {
        configuration.AddBatchConsumer<CreateGroupConsumer, CreateGroupConsumerDefinition>();
        configuration.AddBatchConsumer<DeleteGroupConsumer, DeleteGroupConsumerDefinition>();
        configuration.AddBatchConsumer<SyncGroupsConsumer, SyncGroupsConsumerDefinition>();

        return configuration;
    }
    
    /// <summary>
    ///     Добавление консьюмеров для работы с объявлениями
    /// </summary>
    public static IBusRegistrationConfigurator AddAnnouncementsConsumers(
        this IBusRegistrationConfigurator configuration)
    {
        configuration.AddBatchConsumer<CreateAnnouncementConsumer, CreateAnnouncementConsumerDefinition>();
        
        return configuration;
    }
}