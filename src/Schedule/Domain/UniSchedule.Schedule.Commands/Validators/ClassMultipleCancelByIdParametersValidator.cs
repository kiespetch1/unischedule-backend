using FluentValidation;
using System;
using System.Linq;
using UniSchedule.Schedule.Database;
using UniSchedule.Shared.DTO.Parameters;
using UniSchedule.Validation;
using UniSchedule.Schedule.Entities;

namespace UniSchedule.Schedule.Commands.Validators;

/// <summary>
///     Валидатор для параметров отмены нескольких пар по их идентификаторам.
/// </summary>
public class ClassMultipleCancelByIdParametersValidator : ValidatorBase<ClassMultipleCancelByIdParameters>
{
    /// <summary />
    public ClassMultipleCancelByIdParametersValidator(DatabaseContext context) : base(context)
    {
        RuleFor(x => x.ClassIds)
            .NotEmpty()
            .WithMessage("Список идентификаторов пар не может быть пустым.")
            .Must(IsExists<Class, Guid>)
            .WithMessage("Одна или несколько пар с указанными идентификаторами не найдены.");
    }
}
