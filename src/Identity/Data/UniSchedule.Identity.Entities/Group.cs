using UniSchedule.Abstractions.Entities;

namespace UniSchedule.Identity.Entities;

/// <summary>
///     Учебная группа
/// </summary>
public class Group : Entity<Guid>
{
    /// <summary>
    ///     Название
    /// </summary>
    public required string Name { get; set; }
}