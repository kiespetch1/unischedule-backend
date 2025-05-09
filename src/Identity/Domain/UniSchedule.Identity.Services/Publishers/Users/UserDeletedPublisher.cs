using MassTransit;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Identity.DTO.Messages.Users;

namespace UniSchedule.Identity.Services.Publishers.Users;

/// <summary>
///     Сервис для удаления пользователей через брокер сообщений
/// </summary>
public class UserDeletedPublisher(IBus bus) : PublisherBase<UserMqDeleteParameters>(bus);