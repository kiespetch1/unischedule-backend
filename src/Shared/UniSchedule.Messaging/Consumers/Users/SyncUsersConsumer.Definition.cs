using UniSchedule.Abstractions.Messaging;
using UniSchedule.Identity.DTO.Messages;

namespace UniSchedule.Messaging.Consumers.Users;

/// <summary>
///     Описание для обработчика сообщений
/// </summary>
public class SyncUsersConsumerDefinition :
    ConsumerDefinitionBase<SyncUsersConsumer, UsersMqSyncParameters>
{
    /// <summary />
    public SyncUsersConsumerDefinition() : base("sync_users") { }
}