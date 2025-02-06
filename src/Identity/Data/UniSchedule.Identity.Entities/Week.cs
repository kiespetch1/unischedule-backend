using UniSchedule.Abstractions.Entities;
using UniSchedule.Identity.Entities.Enums;

namespace UniSchedule.Identity.Entities;

/// <summary>
///     Неделя
/// </summary>
public class Week : Entity<Guid>
{
    /// <summary>
    ///     Тип недели
    /// </summary>
    public WeekType WeekType { get; set; }

    /// <summary>
    ///     Подгруппа
    /// </summary>
    public Subgroup Subgroup { get; set; }

    /// <summary>
    ///     Дни недели
    /// </summary>
    public ICollection<Day> Days { get; set; }
}