using UniSchedule.Identity.Entities;
using UniSchedule.Identity.Shared;

namespace UniSchedule.Events.Entities;

/// <summary>
///     Предмет
/// </summary>
public class Subject(SubjectOption code) : HandbookEntry<SubjectOption>(code)
{
    public Subject() : this(default) { }

    /// <summary>
    ///     Идентификатор изменяемой сущности
    /// </summary>
    public Guid SubjectId { get; set; }
}