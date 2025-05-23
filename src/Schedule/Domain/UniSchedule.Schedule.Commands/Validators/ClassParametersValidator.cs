using FluentValidation;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Parameters;
using UniSchedule.Validation;
using Day = UniSchedule.Schedule.Entities.Day;
using Teacher = UniSchedule.Schedule.Entities.Teacher;

namespace UniSchedule.Schedule.Commands.Validators;

/// <summary>
///     Валидатор входных параметров пары
/// </summary>
public class ClassParametersValidator<TParams> : ValidatorBase<TParams> where TParams : ClassUpdateParameters
{
    /// <inheritdoc />
    public ClassParametersValidator(DatabaseContext context) : base(context)
    {
        RuleFor(p => p.StartedAt)
            .LessThan(p => p.FinishedAt)
            .WithMessage("Дата начала должна быть раньше даты окончания");
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Название пары не может быть пустым");

        RuleFor(x => x.TeacherId)
            .Must(IsExist<Teacher, Guid>)
            .WithMessage("Преподаватель не найден");

        RuleFor(x => x.LocationId)
            .Must(IsExist<Location, Guid>)
            .WithMessage("Локация не найдена");
    }
}

/// <summary>
///     Валидатор входных параметров для создания пары
/// </summary>
public class ClassCreateParametersValidator : ClassParametersValidator<ClassCreateParameters>
{
    public ClassCreateParametersValidator(DatabaseContext context) : base(context)
    {
        RuleFor(x => x.DayId)
            .Must(IsExist<Day, Guid>)
            .WithMessage("День не найден");
    }
}

/// <summary>
///     Валидатор входных параметров для обновления пары
/// </summary>
public class ClassUpdateParametersValidator(DatabaseContext context)
    : ClassParametersValidator<ClassUpdateParameters>(context);