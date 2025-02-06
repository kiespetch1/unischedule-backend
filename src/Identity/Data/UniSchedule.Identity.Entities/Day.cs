using UniSchedule.Abstractions.Entities;
using UniSchedule.Identity.Entities.Enums;

namespace UniSchedule.Identity.Entities;

/// <summary>
///     День
/// </summary>
public class Day : Entity<Guid>
{
    /// <summary>
    ///     День недели
    /// </summary>
    public DayOfWeek DayOfWeek { get; set; }

    /// <summary>
    ///     Пары
    /// </summary>
    public ICollection<Class> Classes { get; set; }
}