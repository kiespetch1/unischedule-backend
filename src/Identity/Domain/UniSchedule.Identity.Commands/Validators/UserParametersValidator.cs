using FluentValidation;
using UniSchedule.Identity.Database;
using UniSchedule.Identity.DTO.Parameters;
using UniSchedule.Identity.Entities;
using UniSchedule.Validation;
using Group = UniSchedule.Identity.Entities.Group;

namespace UniSchedule.Identity.Commands.Validators;

public class UserParametersValidator<TParams> : ValidatorBase<TParams> where TParams : RegisterParameters
{
    public UserParametersValidator(DatabaseContext context) : base(context)
    {
        RuleFor(x => x.RoleId)
            .Must(IsExist<Role, Guid>)
            .WithMessage("Роль не найдена");

        RuleFor(x => x.GroupId)
            .Must(x => IsExist<Group, Guid>(x!.Value))
            .When(x => x.GroupId.HasValue)
            .WithMessage("Группа не найдена");

        RuleFor(x => x.ManagedGroupIds)
            .Must(IsExists<Group, Guid>)
            .When(x => x.ManagedGroupIds is not null)
            .WithMessage("Группа не найдена");
    }
}

public class RegisterParametersValidator(DatabaseContext context)
    : UserParametersValidator<RegisterParameters>(context);

public class UserUpdateParametersValidator(DatabaseContext context)
    : UserParametersValidator<UserUpdateParameters>(context);