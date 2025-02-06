using UniSchedule.Abstractions.Entities;

namespace UniSchedule.Identity.Entities;

/// <summary>
///     Преподаватель
/// </summary>
public class Teacher : Entity<Guid>
{
    /// <summary>
    ///     ФИО преподавателя
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Полное ФИО преподавателя
    /// </summary>
    public string FullName { get; set; }
}