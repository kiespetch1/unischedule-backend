using Microsoft.EntityFrameworkCore;
using UniSchedule.Entities;
using UniSchedule.Extensions.Basic;
using UniSchedule.Extensions.Collections;
using UniSchedule.Extensions.DI.Database;
using UniSchedule.Extensions.DI.Settings.Auth;
using UniSchedule.Identity.Entities;
using UniSchedule.Identity.Entities.Owned;
using UniSchedule.Identity.Shared;
using User = UniSchedule.Identity.Entities.User;

namespace UniSchedule.Identity.Database.Helpers;

/// <inheritdoc cref="HandbookSeederBase{TContext}" />
public class DataSeeder(DatabaseContext context, AdminCredentials credentials)
    : HandbookSeederBase<DatabaseContext>(context)
{
    private static readonly Guid mainGroupId = new("d0b8f7e8-c7a9-4f0a-8b1f-07e5a9a2c1e4");

    /// <inheritdoc cref="HandbookSeederBase{TContext}.SeedAsync" />
    public override async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedGroupsAsync();
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

    /// <summary>
    ///     Инициализация групп
    /// </summary>
    private async Task SeedGroupsAsync()
    {
        var groupExists = await context.Groups.AnyAsync(g => g.Id == mainGroupId);
        if (!groupExists)
        {
            var ivtB21Group = new Group { Id = mainGroupId, Name = "ИВТ-Б21" };
            await context.Groups.AddAsync(ivtB21Group);
            await context.SaveChangesAsync();
        }
    }

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

            var group = await context.Groups
                .SingleOrNotFoundAsync(g => g.Id == mainGroupId);

            var admin = CreateAdmin(roles, group.Id);
            var testUser = CreateTestUser(roles, group.Id);

            await context.AddAsync(admin);
            await context.AddAsync(testUser);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    ///     Создание пользователя-администратора
    /// </summary>
    /// <param name="roles">Список ролей</param>
    /// <param name="groupId">ID группы пользователя</param>
    /// <returns>Модель пользователя</returns>
    private User CreateAdmin(Role[] roles, Guid groupId)
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
            GroupId = groupId,
            ManagedGroupIds = [groupId],
            Password = new PasswordInfo { Hash = PasswordUtils.HashPassword(credentials.Password, salt), Salt = salt }
        };

        return user;
    }

    /// <summary>
    ///     Создание тестового пользователя
    /// </summary>
    /// <param name="roles">Список ролей</param>
    /// <param name="groupId">ID группы пользователя</param>
    /// <returns>Модель пользователя</returns>
    private User CreateTestUser(Role[] roles, Guid groupId)
    {
        var salt = PasswordUtils.GenerateSequence();
        var role = roles.Single(x => x.Name == RoleOption.Admin.ToString()); // Test user is also Admin for now

        var user = new User
        {
            Surname = "Тест",
            Name = "Тест",
            Patronymic = "Тест",
            Email = "test@test.ru",
            Role = role,
            RoleId = role.Id,
            GroupId = groupId,
            ManagedGroupIds = [groupId],
            Password = new PasswordInfo { Hash = PasswordUtils.HashPassword("test12345", salt), Salt = salt }
        };

        return user;
    }
    #endregion
}