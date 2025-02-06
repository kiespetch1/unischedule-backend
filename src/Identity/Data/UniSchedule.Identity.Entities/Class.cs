using UniSchedule.Abstractions.Entities;
using UniSchedule.Identity.Entities.Enums;

namespace UniSchedule.Identity.Entities;

/// <summary>
///     Пара
/// </summary>
public class Class : Entity<Guid>
{
    /// <summary>
    ///     Название
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    ///     Время начала
    /// </summary>
    public TimeOnly StartedAt { get; set; }
    
    /// <summary>
    ///     Время окончания
    /// </summary>
    public TimeOnly FinishedAt { get; set; }
    
    /// <summary>
    ///     Тип недели, на которой проводится пара (четная/нечетная)
    /// </summary>
    public WeekType WeekType { get; set; }

    /// <summary>
    ///     Подгруппа, для которой проводится пара
    /// </summary>
    public Subgroup Subgroup { get; set; }
    
    /// <summary>
    ///     Отменена ли пара
    /// </summary>
    public bool IsCancelled { get; set; }
    
}