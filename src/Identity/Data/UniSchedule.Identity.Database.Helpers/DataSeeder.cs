using Microsoft.EntityFrameworkCore;
using UniSchedule.Extensions.Basic;
using UniSchedule.Extensions.DI.Database;
using UniSchedule.Extensions.DI.Settings.Auth;
using UniSchedule.Identity.Entities;
using UniSchedule.Identity.Entities.Owned;
using UniSchedule.Identity.Shared;

namespace UniSchedule.Identity.Database.Helpers;

/// <inheritdoc cref="HandbookSeederBase{TContext}" />
public class DataSeeder(DatabaseContext context, AdminCredentials credentials)
    : HandbookSeederBase<DatabaseContext>(context)
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
            .Select(value => new Role { Id = value.GetId(), Name = value.ToString() })
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
            var testUser = CreateTestUser(roles);

            await context.AddAsync(admin);
            await context.AddAsync(testUser);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    ///     Создание пользователя-администратора
    /// </summary>
    /// <param name="roles">Список ролей</param>
    /// <returns>Модель пользователя</returns>
    private User CreateAdmin(params Role[] roles)
    {
        var salt = PasswordUtils.GenerateSequence();
        var role = roles.Single(x => x.Name == RoleOption.Admin.ToString());

        var user = new User
        {
            Surname = "Админов",
            Name = "Админ",
            Patronymic = "Админович",
            Email = credentials.Email,
            Role = role,
            RoleId = role.Id,
            GroupId = Guid.Empty,
            ManagedGroupIds = [Guid.Empty],
            Password = new PasswordInfo { Hash = PasswordUtils.HashPassword(credentials.Password, salt), Salt = salt }
        };

        return user;
    }

    /// <summary>
    ///     Создание тестового пользователя
    /// </summary>
    /// <param name="roles">Список ролей</param>
    /// <returns>Модель пользователя</returns>
    private User CreateTestUser(params Role[] roles)
    {
        var salt = PasswordUtils.GenerateSequence();
        var role = roles.Single(x => x.Name == RoleOption.Admin.ToString());

        var user = new User
        {
            Surname = "Тест",
            Name = "Тест",
            Patronymic = "Тест",
            Email = "test@test.ru",
            Role = role,
            RoleId = role.Id,
            GroupId = Guid.Empty,
            ManagedGroupIds = [Guid.Empty],
            Password = new PasswordInfo { Hash = PasswordUtils.HashPassword("test12345", salt), Salt = salt }
        };

        return user;
    }
    #endregion
}