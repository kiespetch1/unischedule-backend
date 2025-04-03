using MassTransit;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Identity.DTO.Messages;

namespace UniSchedule.Identity.Services.Publishers;

/// <summary>
///     Сервис для обновления пользователей через брокер сообщений
/// </summary>
/// <param name="bus"></param>
public class UserUpdatedPublisher(IBus bus) : PublisherBase<UserMqUpdateParameters>(bus);