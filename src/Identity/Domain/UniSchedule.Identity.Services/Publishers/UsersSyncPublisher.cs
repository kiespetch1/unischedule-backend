using MassTransit;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Identity.DTO.Messages;

namespace UniSchedule.Identity.Services.Publishers;

/// <summary>
///     Сервис для синхронизации пользователей через брокер сообщений
/// </summary>
public class UsersSyncPublisher(IBus bus) : PublisherBase<UsersMqSyncParameters>(bus);