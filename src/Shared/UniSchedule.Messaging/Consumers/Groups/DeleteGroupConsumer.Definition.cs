using UniSchedule.Abstractions.Messaging;
using UniSchedule.Identity.DTO.Messages.Groups;

namespace UniSchedule.Messaging.Consumers.Groups;

/// <summary>
///     Описание для обработчика сообщений
/// </summary>
public class DeleteGroupConsumerDefinition : ConsumerDefinitionBase<DeleteGroupConsumer, GroupMqDeleteParameters>
{
    /// <summary />
    public DeleteGroupConsumerDefinition() : base("delete_group") { }
}