using FluentValidation;
using UniSchedule.Schedule.Database;
using UniSchedule.Shared.DTO.Parameters;
using UniSchedule.Validation;
using Group = UniSchedule.Schedule.Entities.Group;

namespace UniSchedule.Schedule.Commands.Validators;

/// <summary>
///     Валидатор параметров импорта расписания
/// </summary>
public class ClassScheduleImportParametersValidator : ValidatorBase<ClassScheduleImportParameters>
{
    /// <summary />
    public ClassScheduleImportParametersValidator(DatabaseContext context) : base(context)
    {
        RuleFor(x => x.GroupId)
            .NotEmpty()
            .WithMessage("Идентификатор группы не должен быть пустым")
            .Must(IsExist<Group, Guid>)
            .WithMessage(x => $"Группа с идентификатором '{x.GroupId}' не найдена");

        RuleFor(x => x.Url)
            .NotEmpty()
            .WithMessage("URL не должен быть пустым")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("URL должен быть действительным");
    }
}