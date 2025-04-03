using MassTransit;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Identity.DTO.Messages;

namespace UniSchedule.Identity.Services.Publishers;

/// <summary>
///     Сервис для удаления пользователей через брокер сообщений
/// </summary>
public class UserDeletedPublisher(IBus bus) : PublisherBase<UserMqDeleteParameters>(bus);