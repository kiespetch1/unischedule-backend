using FluentValidation;
using UniSchedule.Events.Database;
using UniSchedule.Events.Shared.Parameters;
using UniSchedule.Validation;

namespace UniSchedule.Events.Commands.Events;

public class EventCreateParametersValidator : ValidatorBase<EventCreateParameters>
{
    public EventCreateParametersValidator(DatabaseContext context) : base(context)
    {
        RuleFor(x => x.ActionId).NotEmpty();
        RuleFor(x => x.SubjectId).NotEmpty();
    }
}