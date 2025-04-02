using FluentValidation;
using UniSchedule.Schedule.Database;
using UniSchedule.Shared.DTO.Parameters;
using UniSchedule.Validation;

namespace UniSchedule.Schedule.Commands.Validators;

/// <summary>
///     Валидатор входных параметров места проведения
/// </summary>
public class LocationParametersValidator<TParams> : ValidatorBase<TParams>
    where TParams : LocationCreateParameters
{
    /// <summary />
    public LocationParametersValidator(DatabaseContext context) : base(context)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Название места проведения не может быть пустым");
    }
}

/// <summary>
///     Валидатор входных параметров для создания места проведения
/// </summary>
public class LocationCreateParametersValidator(DatabaseContext context)
    : LocationParametersValidator<LocationCreateParameters>(context);

/// <summary>
///     Валидатор входных параметров для обновления места проведения
/// </summary>
public class LocationUpdateParametersValidator(DatabaseContext context)
    : LocationParametersValidator<LocationUpdateParameters>(context);