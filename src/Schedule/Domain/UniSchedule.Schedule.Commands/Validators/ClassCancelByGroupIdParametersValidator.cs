using FluentValidation;
using UniSchedule.Schedule.Database;
using UniSchedule.Shared.DTO.Parameters;
using UniSchedule.Validation;
using Group = UniSchedule.Schedule.Entities.Group;

namespace UniSchedule.Schedule.Commands.Validators;

/// <summary>
///     Валидатор для параметров отмены всех пар группы
/// </summary>
public class ClassCancelByGroupIdParametersValidator : ValidatorBase<ClassCancelByGroupIdParameters>
{
    /// <summary />
    public ClassCancelByGroupIdParametersValidator(DatabaseContext context) : base(context)
    {
        RuleFor(x => x.GroupId)
            .NotEmpty()
            .WithMessage("Идентификатор группы не может быть пустым.")
            .Must(IsExist<Group, Guid>)
            .WithMessage("Группа с указанным идентификатором не найдена.");
    }
}
