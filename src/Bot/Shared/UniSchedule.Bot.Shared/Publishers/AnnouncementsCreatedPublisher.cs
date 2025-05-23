using MassTransit;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Bot.Shared.Announcements;

namespace UniSchedule.Bot.Shared.Publishers;

/// <summary>
///     Сервис для публикации объявлений в брокер сообщений
/// </summary>
public class AnnouncementsCreatedPublisher(IBus bus) : PublisherBase<AnnouncementMqCreateParameters>(bus);
