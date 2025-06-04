using FluentValidation;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Parameters;
using UniSchedule.Validation;

namespace UniSchedule.Schedule.Commands.Validators;

/// <summary>
///     Валидатор для параметров восстановления нескольких пар
/// </summary>
public class ClassMultipleRestoreParametersValidator : ValidatorBase<ClassMultipleRestoreParameters>
{
    /// <summary />
    public ClassMultipleRestoreParametersValidator(DatabaseContext context) : base(context)
    {
        RuleFor(p => p.ClassIds)
            .NotEmpty()
            .WithMessage("Список идентификаторов пар не может быть пустым")
            .Must(IsExists<Class, Guid>)
            .WithMessage("Одна или несколько пар с указанными идентификаторами не найдены");
    }
}