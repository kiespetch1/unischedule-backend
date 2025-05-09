using UniSchedule.Abstractions.Entities;

namespace UniSchedule.Identity.DTO.Messages.Groups;

/// <summary>
///     Учебная группа
/// </summary>
public class GroupMqModel : Entity<Guid>
{
    /// <summary>
    ///     Название
    /// </summary>
    public required string Name { get; set; }
}