using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UniSchedule.Schedule.Database;
using UniSchedule.Shared.DTO.Parameters;
using UniSchedule.Validation;

namespace UniSchedule.Schedule.Commands.Validators;

/// <summary>
///     Валидатор параметров импорта расписания
/// </summary>
public class ClassScheduleParametersValidator : ValidatorBase<ClassScheduleImportParameters>
{
    /// <summary />
    public ClassScheduleParametersValidator(DatabaseContext context) : base(context)
    {
        RuleFor(x => x.GroupId)
            .NotEmpty()
            .WithMessage("Идентификатор группы не должен быть пустым")
            .MustAsync(async (groupId, cancellationToken) =>
                await context.Groups.AnyAsync(g => g.Id == groupId, cancellationToken))
            .WithMessage(x => $"Группа с идентификатором '{x.GroupId}' не найдена");

        RuleFor(x => x.Url)
            .NotEmpty()
            .WithMessage("URL не должен быть пустым")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("URL должен быть действительным");
    }
}