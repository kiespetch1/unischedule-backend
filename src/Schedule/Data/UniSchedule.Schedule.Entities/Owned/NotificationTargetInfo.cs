namespace UniSchedule.Schedule.Entities.Owned;

/// <summary>
///     Информация о получателях уведомления
/// </summary>
public class NotificationTargetInfo
{
    /// <summary>
    ///     Включенные курсы
    /// </summary>
    public List<int>? IncludedGrades { get; set; }

    /// <summary>
    ///     Включенный набор групп
    /// </summary>
    public List<Guid>? IncludedGroups { get; set; }

    /// <summary>
    ///     Включенные кафедры
    /// </summary>
    public List<Guid>? Department { get; set; }

    /// <summary>
    ///     Исключенные курсы
    /// </summary>
    public List<int>? ExcludedGrades { get; set; }

    /// <summary>
    ///     Исключенный набор групп
    /// </summary>
    public List<Guid>? ExcludedGroups { get; set; }

    /// <summary>
    ///     Исключенные кафедры
    /// </summary>
    public List<Guid>? ExcludedDepartments { get; set; }
}