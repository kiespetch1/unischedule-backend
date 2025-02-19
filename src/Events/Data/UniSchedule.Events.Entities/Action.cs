using UniSchedule.Identity.Entities;
using UniSchedule.Identity.Shared;

namespace UniSchedule.Events.Entities;

/// <summary>
///     Действие
/// </summary>
public class Action(ActionOption code) : HandbookEntry<ActionOption>(code)
{
    public Action() : this(default) { }
}