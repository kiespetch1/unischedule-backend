using MassTransit;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Identity.DTO.Messages.Users;

namespace UniSchedule.Identity.Services.Publishers.Users;

/// <summary>
///     Сервис для обновления пользователей через брокер сообщений
/// </summary>
/// <param name="bus"></param>
public class UserUpdatedPublisher(IBus bus) : PublisherBase<UserMqUpdateParameters>(bus);