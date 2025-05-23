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
            .When(x => x.Target is { IncludedGroups.Count: > 0 })
            .WithMessage("Не все включенные группы существуют");

        RuleFor(x => x.Target)
            .Must(x => x != null && IsExists<Group, Guid>(x.ExcludedGroups))
            .When(x => x.Target is { ExcludedGroups.Count: > 0 })
            .WithMessage("Не все исключенные группы существуют");

        RuleFor(x => x.Target)
            .Must(x => x != null && x.IncludedGrades!.Any(grade => grade is > 0 and <= 4))
            .When(x => x.Target is { IncludedGrades.Count: > 0 })
            .WithMessage("Включенные курсы должны быть в диапазоне от 1 до 4");

        RuleFor(x => x.Target)
            .Must(x => x != null && x.ExcludedGrades!.Any(grade => grade is > 0 and <= 4))
            .When(x => x.Target is { ExcludedGrades.Count: > 0 })
            .WithMessage("Исключенные курсы должны быть в диапазоне от 1 до 4");

        RuleFor(x => x.AvailableUntil)
            .NotNull()
            .When(x => x.IsTimeLimited)
            .WithMessage("Если объявление ограничено по времени, необходимо указать дату окончания");

        RuleFor(x => x.AvailableUntil)
            .Null()
            .When(x => !x.IsTimeLimited)
            .WithMessage("Дата окончания должна быть указана только для объявлений с ограничением по времени");

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