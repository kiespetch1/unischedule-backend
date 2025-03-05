using Microsoft.EntityFrameworkCore;
using UniSchedule.Extensions.DI.Database;
using UniSchedule.Identity.Entities;
using UniSchedule.Identity.Entities.Owned;
using UniSchedule.Identity.Shared;

namespace UniSchedule.Identity.Database.Helpers;

/// <inheritdoc cref="HandbookSeederBase{TContext}" />
public class DataSeeder(DatabaseContext context) : HandbookSeederBase<DatabaseContext>(context)
{
    /// <inheritdoc cref="HandbookSeederBase{TContext}.SeedAsync" />
    public override async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedUsersAsync();
    }

    #region [ Roles ]

    /// <summary>
    ///     Инициализация ролей
    /// </summary>
    private async Task SeedRolesAsync()
    {
        var existingEntities = await context.Roles
            .ToListAsync();
        var entities = Enum.GetValues<RoleOption>()
            .Select(value => new Role { Name = value.ToString() })
            .ToList();

        var entitiesToCreate = entities.Where(a =>
            !existingEntities.Select(x => x.Name).Contains(a.Name));
        var entitiesToUpdate = existingEntities
            .Where(a => entities.Select(x => x.Name).Contains(a.Name))
            .Select(a => UpdateDefaultRole(a, entities.Single(x => a.Name == x.Name)));

        await context.Roles.AddRangeAsync(entitiesToCreate);
        context.Roles.UpdateRange(entitiesToUpdate);

        await context.SaveChangesAsync();
    }

    /// <summary>
    ///     Обновление роли
    /// </summary>
    /// <param name="toUpdate">Роль, которую нужно обновить</param>
    /// <param name="updated">Обновленная роль</param>
    /// <returns>Обновленная роль</returns>
    private static Role UpdateDefaultRole(Role toUpdate, Role updated)
    {
        toUpdate.Name = updated.Name;

        return toUpdate;
    }

    #endregion

    #region [ Users ]

    /// <summary>
    ///     Инициализация пользователей
    /// </summary>
    private async Task SeedUsersAsync()
    {
        var isExists = await context.Users.AnyAsync();
        if (!isExists)
        {
            var roles = await context.Roles
                .ToArrayAsync();

            var admin = CreateAdmin(roles);

            await context.AddAsync(admin);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    ///     Создание пользователя-администратора
    /// </summary>
    /// <param name="roles">Список ролей</param>
    /// <returns>Модель пользователя</returns>
    private static User CreateAdmin(params Role[] roles)
    {
        var salt = PasswordUtils.GenerateSequence();
        var password = "admin12345";
        var role = roles.Single(x => x.Name == RoleOption.Admin.ToString());

        var user = new User
        {
            Surname = "Админов",
            Name = "Админ",
            Patronymic = "Админович",
            Email = "admin@test.ru",
            Role = role,
            RoleId = role.Id,
            GroupId = Guid.Empty,
            ManagedGroupIds = [Guid.Empty],
            Password = new PasswordInfo { Hash = PasswordUtils.HashPassword(password, salt), Salt = salt }
        };

        return user;
    }

    #endregion
}