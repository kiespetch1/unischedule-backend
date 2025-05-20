using UniSchedule.Abstractions.Messaging;
using UniSсhedule.Bot.Shared.Announcements;

namespace UniSchedule.Messaging.Consumers.Announcements;

/// <summary>
///     Описание для обработчика сообщений создания объявлений
/// </summary>
public class CreateAnnouncementConsumerDefinition : ConsumerDefinitionBase<CreateAnnouncementConsumer, AnnouncementMqCreateParameters>
{
    /// <summary />
    public CreateAnnouncementConsumerDefinition() : base("create_announcement") { }
}