using Microsoft.EntityFrameworkCore;
using UniSchedule.Abstractions.Helpers;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Identity.DTO.Messages.Groups;
using UniSchedule.Schedule.Database;

namespace UniSchedule.Schedule.Services;

/// <summary>
///     Сервис для синхронизации пользователей между сервисами
/// </summary>
public class GroupsSyncService(DatabaseContext context, IPublisher<GroupsMqSyncParameters> publisher) : ISyncService
{
    /// <inheritdoc />
    public async Task SyncAsync()
    {
        var groups = await context.Groups
            .AsNoTracking()
            .Select(u => new GroupMqModel { Id = u.Id, Name = u.Name })
            .ToListAsync();

        var data = new GroupsMqSyncParameters { Groups = groups };
        await publisher.PublishAsync(data);
    }
}