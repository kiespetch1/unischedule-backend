using UniSchedule.Abstractions.Entities;

namespace UniSchedule.Schedule.Entities;

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