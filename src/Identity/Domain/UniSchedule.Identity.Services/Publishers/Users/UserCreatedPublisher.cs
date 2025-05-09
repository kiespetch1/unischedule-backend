using MassTransit;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Identity.DTO.Messages.Users;

namespace UniSchedule.Identity.Services.Publishers.Users;

/// <summary>
///     Сервис для создания пользователей через брокер сообщений
/// </summary>
public class UserCreatedPublisher(IBus bus) : PublisherBase<UserMqCreateParameters>(bus);