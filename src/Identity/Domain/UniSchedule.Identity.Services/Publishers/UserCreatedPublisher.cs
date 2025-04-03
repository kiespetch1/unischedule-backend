using MassTransit;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Identity.DTO.Messages;

namespace UniSchedule.Identity.Services.Publishers;

/// <summary>
///     Сервис для создания пользователей через брокер сообщений
/// </summary>
public class UserCreatedPublisher(IBus bus) : PublisherBase<UserMqCreateParameters>(bus);