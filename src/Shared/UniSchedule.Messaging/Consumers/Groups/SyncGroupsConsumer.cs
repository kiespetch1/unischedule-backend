using MassTransit;
using Microsoft.EntityFrameworkCore;
using UniSchedule.Entities;
using UniSchedule.Identity.DTO.Messages.Groups;
using UniSchedule.Identity.Entities;

namespace UniSchedule.Messaging.Consumers.Groups;

/// <summary>
///     Консьюмер для синхронизации групп
/// </summary>
public class SyncGroupsConsumer(IDbContextAccessor dbContextAccessor) : IConsumer<Batch<GroupsMqSyncParameters>>
{
    /// <inheritdoc />
    public async Task Consume(ConsumeContext<Batch<GroupsMqSyncParameters>> context)
    {
        var parametersCollection = context.Message
            .Select(x => x.Message)
            .ToList();

        foreach (var parameters in parametersCollection)
        {
            await DeleteAsync(parameters.Groups);
            await UpdateAsync(parameters.Groups);
            await CreateAsync(parameters.Groups);
        }
    }

    // есть входящий и существующий списки
    // существующий список не содержит входящих значений - удаление лишнего
    // существующий список содержит входящие значения - обновление
    // входящий список не содержит существующих значений - создание

    private async Task DeleteAsync(List<GroupMqModel> groups)
    {
        var dbContext = dbContextAccessor.GetDbContext();
        var groupIds = groups.Select(x => x.Id).ToList();
        var deletedGroups = await dbContext.Set<Group>()
            .Where(x => !groupIds.Contains(x.Id))
            .ToListAsync();

        dbContext.Set<Group>().RemoveRange(deletedGroups);
        await dbContext.SaveChangesAsync();
    }

    private async Task UpdateAsync(List<GroupMqModel> groups)
    {
        var dbContext = dbContextAccessor.GetDbContext();
        var userIds = groups.Select(x => x.Id).ToList();
        var updatedGroups = await dbContext.Set<Group>()
            .Where(x => userIds.Contains(x.Id))
            .ToListAsync();

        foreach (var updatedGroup in updatedGroups)
        {
            var group = groups.Single(x => x.Id == updatedGroup.Id);

            updatedGroup.Name = group.Name;
            updatedGroup.UsedMessenger = group.UsedMessenger;
        }

        await dbContext.SaveChangesAsync();
    }

    private async Task CreateAsync(List<GroupMqModel> users)
    {
        var dbContext = dbContextAccessor.GetDbContext();
        var groupIds = await dbContext.Set<Group>().Select(x => x.Id).ToListAsync();
        var createdGroups = users
            .Where(x => !groupIds.Contains(x.Id))
            .Select(x => new Group { Id = x.Id, Name = x.Name, UsedMessenger = x.UsedMessenger})
            .ToList();

        await dbContext.Set<Group>().AddRangeAsync(createdGroups);
        await dbContext.SaveChangesAsync();
    }
}