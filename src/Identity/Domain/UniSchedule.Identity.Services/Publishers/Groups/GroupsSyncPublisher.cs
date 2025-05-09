using MassTransit;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Identity.DTO.Messages.Groups;

namespace UniSchedule.Identity.Services.Publishers.Groups;

/// <summary>
///     Сервис для синхронизации групп через брокер сообщений
/// </summary>
public class GroupsSyncPublisher(IBus bus) : PublisherBase<GroupsMqSyncParameters>(bus);