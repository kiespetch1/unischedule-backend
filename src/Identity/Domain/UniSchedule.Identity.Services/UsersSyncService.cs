using Microsoft.EntityFrameworkCore;
using UniSchedule.Abstractions.Helpers;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Identity.Database;
using UniSchedule.Identity.DTO.Messages;

namespace UniSchedule.Identity.Services;

/// <summary>
///     Сервис для синхронизации пользователей между сервисами
/// </summary>
public class UsersSyncService(DatabaseContext context, IPublisher<UsersMqSyncParameters> publisher) : ISyncService
{
    /// <inheritdoc />
    public async Task SyncAsync()
    {
        var users = await context.Users
            .AsNoTracking()
            .Select(u => new UserMqModel
            {
                Id = u.Id,
                Surname = u.Surname,
                Name = u.Name,
                Patronymic = u.Patronymic,
                Email = u.Email
            })
            .ToListAsync();

        var data = new UsersMqSyncParameters { Users = users };
        await publisher.PublishAsync(data);
    }
}