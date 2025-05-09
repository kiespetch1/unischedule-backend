using MassTransit;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Identity.DTO.Messages.Groups;

namespace UniSchedule.Identity.Services.Publishers.Groups;

/// <summary>
///     Сервис для создания групп через брокер сообщений
/// </summary>
public class GroupCreatedPublisher(IBus bus) : PublisherBase<GroupMqCreateParameters>(bus);