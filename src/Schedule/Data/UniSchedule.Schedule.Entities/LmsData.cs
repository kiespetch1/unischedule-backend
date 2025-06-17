using UniSchedule.Abstractions.Entities;

namespace UniSchedule.Schedule.Entities;

/// <summary>
///     Данные для доступа к LMS
/// </summary>
public class LmsData : Entity<Guid>
{
    /// <summary>
    ///     Название предмета
    /// </summary>
    public required string Subject { get; set; }

    /// <summary>
    ///     Данные доступа
    /// </summary>
    public required string Data { get; set; }

    /// <summary>
    ///     Идентификатор группы
    /// </summary>
    public Guid GroupId { get; set; }

    /// <summary>
    ///     Группа
    /// </summary>
    public Group Group { get; set; }
}