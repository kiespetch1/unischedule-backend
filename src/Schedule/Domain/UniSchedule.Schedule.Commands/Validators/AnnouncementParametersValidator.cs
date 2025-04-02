using FluentValidation;
using UniSchedule.Schedule.Database;
using UniSchedule.Shared.DTO.Parameters;
using UniSchedule.Validation;
using Group = UniSchedule.Schedule.Entities.Group;

namespace UniSchedule.Schedule.Commands.Validators;

/// <summary>
///     Валидатор входных параметров объявления
/// </summary>
public class AnnouncementParametersValidator<TParams> : ValidatorBase<TParams>
    where TParams : AnnouncementCreateParameters
{
    /// <summary />
    public AnnouncementParametersValidator(DatabaseContext context) : base(context)
    {
        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Текст объявления не может быть пустым");

        RuleFor(x => x.Target)
            .Must(x => x != null && IsExists<Group, Guid>(x.IncludedGroups))
            .When(x => x.Target is { IncludedGroups.Count: > 0 });

        RuleFor(x => x.Target)
            .Must(x => x != null && IsExists<Group, Guid>(x.ExcludedGroups))
            .When(x => x.Target is { ExcludedGroups.Count: > 0 });

        //TODO: аналогично нужно сделать валидацию для кафедр/отделений когда до них дойдет дело
    }
}

/// <summary>
///     Валидатор входных параметров для создания объявления
/// </summary>
public class AnnouncementCreateParametersValidator(DatabaseContext context)
    : AnnouncementParametersValidator<AnnouncementCreateParameters>(context);

/// <summary>
///     Валидатор входных параметров для обновления объявления
/// </summary>
public class AnnouncementUpdateParametersValidator(DatabaseContext context)
    : AnnouncementParametersValidator<AnnouncementUpdateParameters>(context);