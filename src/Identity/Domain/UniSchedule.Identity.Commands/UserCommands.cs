using Microsoft.EntityFrameworkCore;
using UniSchedule.Abstractions.Commands;
using UniSchedule.Extensions.Collections;
using UniSchedule.Identity.Database;
using UniSchedule.Identity.DTO.Parameters;
using UniSchedule.Identity.Entities;
using UniSchedule.Identity.Entities.Owned;

namespace UniSchedule.Identity.Commands;

/// <summary>
///     Команды для работы с пользователями
/// </summary>
public class UserCommands(DatabaseContext context) :
    ICreateCommand<User, RegisterParameters, Guid>,
    IUpdateCommand<User, UserUpdateParameters, Guid>
{
    /// <inheritdoc />
    public async Task<Guid> ExecuteAsync(RegisterParameters parameters, CancellationToken cancellationToken = default)
    {
        var role = await context.Roles
            .SingleOrNotFoundAsync(r => r.Id == parameters.RoleId, cancellationToken);

        var salt = PasswordUtils.GenerateSequence();

        var user = new User
        {
            Surname = parameters.Surname,
            Name = parameters.Name,
            Patronymic = parameters.Patronymic,
            Email = parameters.Email,
            Role = role,
            RoleId = role.Id,
            GroupId = parameters.GroupId,
            ManagedGroupIds = parameters.ManagedGroupIds,
            Password = new PasswordInfo
            {
                Hash = PasswordUtils.HashPassword(parameters.Password, salt), Salt = salt
            }
        };

        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(Guid id, UserUpdateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .Include(x => x.Role)
            .SingleOrNotFoundAsync(id, cancellationToken);

        user.Surname = parameters.Surname;
        user.Name = parameters.Name;
        user.Patronymic = parameters.Patronymic;
        user.Email = parameters.Email;
        user.RoleId = parameters.RoleId;
        user.GroupId = parameters.GroupId;
        user.ManagedGroupIds = parameters.ManagedGroupIds;
        var salt = PasswordUtils.GenerateSequence();
        user.Password = new PasswordInfo { Hash = PasswordUtils.HashPassword(parameters.Password, salt), Salt = salt };
        await context.SaveChangesAsync(cancellationToken);
    }
}