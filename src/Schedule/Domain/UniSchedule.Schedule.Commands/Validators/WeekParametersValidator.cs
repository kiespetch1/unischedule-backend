using FluentValidation;
using UniSchedule.Schedule.Database;
using UniSchedule.Shared.DTO.Parameters;
using UniSchedule.Validation;
using Group = UniSchedule.Schedule.Entities.Group;

namespace UniSchedule.Schedule.Commands.Validators;

/// <summary>
///     Валидатор параметров создания недели
/// </summary>
public class WeekParametersValidator<TParams> : ValidatorBase<TParams>
    where TParams : WeekCreateParameters
{
    /// <summary />
    public WeekParametersValidator(DatabaseContext context) : base(context)
    {
        RuleFor(x => x.GroupId)
            .Must(IsExist<Group, Guid>)
            .WithMessage("Группа не найдена");

        RuleFor(x => x.WeekType)
            .NotEmpty()
            .WithMessage("Тип недели не может быть пустым");

        RuleFor(x => x.Subgroup)
            .NotEmpty()
            .WithMessage("Подгруппа не может быть пустой");
    }
}