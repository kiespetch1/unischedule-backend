using UniSchedule.Abstractions.Messaging;
using UniSchedule.Identity.DTO.Messages.Users;

namespace UniSchedule.Messaging.Consumers.Users;

/// <summary>
///     Описание для обработчика сообщений
/// </summary>
public class UpdateUserConsumerDefinition :
    ConsumerDefinitionBase<UpdateUserConsumer, UserMqUpdateParameters>
{
    /// <summary />
    public UpdateUserConsumerDefinition() : base("update_user") { }
}