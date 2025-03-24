using UniSchedule.Abstractions.Entities;
using UniSchedule.Schedule.Entities.Enums;

namespace UniSchedule.Schedule.Entities;

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
    ///     Идентификатор группы
    /// </summary>
    public Guid GroupId { get; set; }

    /// <summary>
    ///     Группа
    /// </summary>
    public Group Group { get; set; }

    /// <summary>
    ///     Дни недели
    /// </summary>
    public ICollection<Day> Days { get; set; }
}