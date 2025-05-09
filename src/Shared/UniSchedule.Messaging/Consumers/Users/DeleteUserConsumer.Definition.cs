using UniSchedule.Abstractions.Messaging;
using UniSchedule.Identity.DTO.Messages.Users;

namespace UniSchedule.Messaging.Consumers.Users;

/// <summary>
///     Описание для обработчика сообщений
/// </summary>
public class DeleteUserConsumerDefinition :
    ConsumerDefinitionBase<DeleteUserConsumer, UserMqDeleteParameters>
{
    /// <summary />
    public DeleteUserConsumerDefinition() : base("delete_user") { }
}