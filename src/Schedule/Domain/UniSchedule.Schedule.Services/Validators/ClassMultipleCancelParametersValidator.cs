using FluentValidation;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Parameters;
using UniSchedule.Validation;

namespace UniSchedule.Schedule.Services.Validators;

/// <summary>
///     Валидатор входных параметров для отмены нескольких пар
/// </summary>
public class ClassMultipleCancelParametersValidator : ValidatorBase<ClassMultipleCancelByDayIdParameters>
{
    public ClassMultipleCancelParametersValidator(DatabaseContext context) : base(context)
    {
        RuleFor( x=> x.DayIds)
            .Must(IsExists<Day, Guid>)
            .WithMessage("Не удалось найти некоторые дни");
    }
}