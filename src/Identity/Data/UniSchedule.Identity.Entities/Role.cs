using UniSchedule.Abstractions.Entities;

namespace UniSchedule.Identity.Entities;

/// <summary>
///     Роль пользователя
/// </summary>
public class Role : Entity<Guid>
{
    public string Name { get; set; }
}