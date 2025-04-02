using FluentValidation;
using UniSchedule.Schedule.Database;
using UniSchedule.Shared.DTO.Parameters;
using UniSchedule.Validation;
using Week = UniSchedule.Schedule.Entities.Week;

namespace UniSchedule.Schedule.Commands.Validators;

/// <summary>
///     Валидатор входных параметров дня
/// </summary>
public class DayParametersValidator<TParams> : ValidatorBase<TParams>
    where TParams : DayCreateParameters
{
    /// <summary />
    public DayParametersValidator(DatabaseContext context) : base(context)
    {
        RuleFor(x => x.WeekId)
            .Must(IsExist<Week, Guid>)
            .WithMessage("Неделя не найдена");

        RuleFor(x => x.DayOfWeek)
            .NotEmpty()
            .WithMessage("День недели не может быть пустым");
    }
}