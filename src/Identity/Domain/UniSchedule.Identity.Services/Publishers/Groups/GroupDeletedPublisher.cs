using MassTransit;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Identity.DTO.Messages.Groups;

namespace UniSchedule.Identity.Services.Publishers.Groups;

/// <summary>
///     Сервис для удаления групп через брокер сообщений
/// </summary>
public class GroupDeletedPublisher(IBus bus) : PublisherBase<GroupMqDeleteParameters>(bus);