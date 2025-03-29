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
    public required DayOfWeek DayOfWeek { get; set; }

    /// <summary>
    ///     Идентификатор недели
    /// </summary>
    public required Guid WeekId { get; set; }

    /// <summary>
    ///     Неделя
    /// </summary>
    public Week Week { get; set; }

    /// <summary>
    ///     Пары
    /// </summary>
    public ICollection<Class> Classes { get; set; }
}