using MassTransit;
using UniSchedule.Abstractions.Messaging;
using UniSсhedule.Bot.Shared.Announcements;

namespace UniSсhedule.Bot.Shared.Publishers;

/// <summary>
///     Сервис для публикации объявлений в брокер сообщений
/// </summary>
public class AnnouncementsCreatedPublisher(IBus bus) : PublisherBase<AnnouncementMqCreateParameters>(bus);
