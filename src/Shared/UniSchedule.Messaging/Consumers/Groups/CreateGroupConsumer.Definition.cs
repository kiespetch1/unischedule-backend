using UniSchedule.Abstractions.Messaging;
using UniSchedule.Identity.DTO.Messages.Groups;

namespace UniSchedule.Messaging.Consumers.Groups;

/// <summary>
///     Описание для обработчика сообщений
/// </summary>
public class CreateGroupConsumerDefinition : ConsumerDefinitionBase<CreateGroupConsumer, GroupMqCreateParameters>
{
    /// <summary />
    public CreateGroupConsumerDefinition() : base("create_group") { }
}