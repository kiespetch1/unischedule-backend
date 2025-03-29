using UniSchedule.Abstractions.Entities;
using UniSchedule.Schedule.Entities.Enums;

namespace UniSchedule.Schedule.Entities;

/// <summary>
///     Место проведения занятий
/// </summary>
public class Location : Entity<Guid>
{
    /// <summary>
    ///     Название
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     Ссылка на встречу
    /// </summary>
    public string? Link { get; set; }

    /// <summary>
    ///     Тип
    /// </summary>
    public required LocationType LocationType { get; set; }

    /// <summary>
    ///     Пары
    /// </summary>
    public ICollection<Class> Classes { get; set; }
}