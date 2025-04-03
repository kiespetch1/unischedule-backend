using UniSchedule.Abstractions.Messaging;
using UniSchedule.Identity.DTO.Messages;

namespace UniSchedule.Messaging.Consumers.Users;

/// <summary>
///     Описание для обработчика сообщений
/// </summary>
public class CreateUserConsumerDefinition :
    ConsumerDefinitionBase<CreateUserConsumer, UserMqCreateParameters>
{
    /// <summary />
    public CreateUserConsumerDefinition() : base("create_user") { }
}