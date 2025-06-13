using FluentValidation;
using UniSchedule.Schedule.Database;
using UniSchedule.Shared.DTO.Parameters;
using UniSchedule.Validation;

namespace UniSchedule.Schedule.Commands.Validators;

/// <summary>
///     Валидатор входных параметров преподавателя
/// </summary>
public class TeacherParametersValidator<TParams> : ValidatorBase<TParams>
    where TParams : TeacherCreateParameters
{
    /// <summary />
    public TeacherParametersValidator(DatabaseContext context) : base(context)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("ФИО преподавателя не может быть пустым");
    }
}

/// <summary>
///     Валидатор входных параметров для создания преподавателя
/// </summary>
public class TeacherCreateParametersValidator(DatabaseContext context)
    : TeacherParametersValidator<TeacherCreateParameters>(context);

/// <summary>
///     Валидатор входных параметров для обновления преподавателя
/// </summary>
public class TeacherUpdateParametersValidator(DatabaseContext context)
    : TeacherParametersValidator<TeacherUpdateParameters>(context);