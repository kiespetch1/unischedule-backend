using UniSchedule.Abstractions.Entities;
using UniSchedule.Entities;

namespace UniSchedule.Identity.DTO.Messages.Groups;

/// <summary>
///     Модель учебной группы для передачи через брокер сообщений
/// </summary>
public class GroupMqModel : Entity<Guid>
{
    /// <summary>
    ///     Название
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     Используемый мессенджер
    /// </summary>
    public required MessengerType UsedMessenger { get; set; }
}