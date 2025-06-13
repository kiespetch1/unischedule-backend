namespace UniSchedule.Schedule.Entities.Owned;

/// <summary>
///     Информация о получателях уведомления
/// </summary>
public class AnnouncementTargetInfo
{
    /// <summary>
    ///     Включенные курсы
    /// </summary>
    public required List<int> IncludedGrades { get; set; }

    /// <summary>
    ///     Включенные группы
    /// </summary>
    public required List<Guid> IncludedGroups { get; set; }

    /// <summary>
    ///     Включенные кафедры
    /// </summary>
    public required List<Guid> IncludedDepartments { get; set; }

    /// <summary>
    ///     Исключенные курсы
    /// </summary>
    public required List<int> ExcludedGrades { get; set; }

    /// <summary>
    ///     Исключенные группы
    /// </summary>
    public required List<Guid> ExcludedGroups { get; set; }

    /// <summary>
    ///     Исключенные кафедры
    /// </summary>
    public required List<Guid> ExcludedDepartments { get; set; }
}