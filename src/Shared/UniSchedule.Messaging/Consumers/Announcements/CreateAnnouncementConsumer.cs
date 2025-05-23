using MassTransit;
using UniSchedule.Schedule.Entities;
using UniSchedule.Bot.Shared.Announcements;

namespace UniSchedule.Messaging.Consumers.Announcements;

/// <summary>
///     Консьюмер для создания объявлений
/// </summary>
public class CreateAnnouncementConsumer(IDbContextAccessor dbContextAccessor) : IConsumer<Batch<AnnouncementMqCreateParameters>>
{
    /// <inheritdoc />
    public async Task Consume(ConsumeContext<Batch<AnnouncementMqCreateParameters>> context)
    {
        var dbContext = dbContextAccessor.GetDbContext();
        var parameters = context.Message.Select(x => x.Message);

        foreach (var parameter in parameters)
        {
            var announcement = new Announcement
            {
                Id = parameter.Id,
                Message = parameter.Message,
                Target = parameter.Target,
                Priority = parameter.Priority,
                IsAnonymous = parameter.IsAnonymous,
                IsTimeLimited = parameter.IsTimeLimited,
                AvailableUntil = parameter.AvailableUntil,
                IsAddedUsingBot = parameter.IsAddedUsingBot,
                CreatedBy = parameter.CreatedBy
            };

            await dbContext.Set<Announcement>().AddAsync(announcement);
            await dbContext.SaveChangesAsync();
        }
    }
}