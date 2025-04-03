using MassTransit;
using Microsoft.EntityFrameworkCore;
using UniSchedule.Entities;
using UniSchedule.Identity.DTO.Messages;

namespace UniSchedule.Messaging.Consumers.Users;

/// <summary>
///     Консьюмер для синхронизации пользователей
/// </summary>
public class SyncUsersConsumer(IDbContextAccessor dbContextAccessor)
    : IConsumer<Batch<UsersMqSyncParameters>>
{
    /// <inheritdoc />
    public async Task Consume(ConsumeContext<Batch<UsersMqSyncParameters>> context)
    {
        var parametersCollection = context.Message
            .Select(x => x.Message)
            .ToList();

        foreach (var parameters in parametersCollection)
        {
            await DeleteAsync(parameters.Users);
            await UpdateAsync(parameters.Users);
            await CreateAsync(parameters.Users);
        }
    }

    // есть входящий и существующий списки
    // существующий список не содержит входящих значений - удаление лишнего
    // существующий список содержит входящие значения - обновление
    // входящий список не содержит существующих значений - создание

    private async Task DeleteAsync(List<UserMqModel> users)
    {
        var dbContext = dbContextAccessor.GetDbContext();
        var userIds = users.Select(x => x.Id).ToList();
        var deletedUsers = await dbContext.Set<User>()
            .Where(x => !userIds.Contains(x.Id))
            .ToListAsync();

        dbContext.Set<User>().RemoveRange(deletedUsers);
        await dbContext.SaveChangesAsync();
    }

    private async Task UpdateAsync(List<UserMqModel> users)
    {
        var dbContext = dbContextAccessor.GetDbContext();
        var userIds = users.Select(x => x.Id).ToList();
        var updatedUsers = await dbContext.Set<User>()
            .Where(x => userIds.Contains(x.Id))
            .ToListAsync();

        foreach (var updatedUser in updatedUsers)
        {
            var user = users.Single(x => x.Id == updatedUser.Id);

            updatedUser.Surname = user.Surname;
            updatedUser.Name = user.Name;
            updatedUser.Patronymic = user.Patronymic;
            updatedUser.Email = user.Email;
        }

        await dbContext.SaveChangesAsync();
    }

    private async Task CreateAsync(List<UserMqModel> users)
    {
        var dbContext = dbContextAccessor.GetDbContext();
        var userIds = await dbContext.Set<User>().Select(x => x.Id).ToListAsync();
        var createdUsers = users
            .Where(x => !userIds.Contains(x.Id))
            .Select(x => new User
            {
                Id = x.Id,
                Surname = x.Surname,
                Name = x.Name,
                Patronymic = x.Patronymic,
                Email = x.Email
            })
            .ToList();

        await dbContext.Set<User>().AddRangeAsync(createdUsers);
        await dbContext.SaveChangesAsync();
    }
}