using MassTransit;
using UniSchedule.Entities;
using UniSchedule.Identity.DTO.Messages.Groups;
using UniSchedule.Identity.Entities;

namespace UniSchedule.Messaging.Consumers.Groups;

/// <summary>
///     Консьюмер для создания групп
/// </summary>
public class CreateGroupConsumer(IDbContextAccessor dbContextAccessor) : IConsumer<Batch<GroupMqCreateParameters>>
{
    /// <inheritdoc />
    public async Task Consume(ConsumeContext<Batch<GroupMqCreateParameters>> context)
    {
        var dbContext = dbContextAccessor.GetDbContext();
        var parametersCollection = context.Message.Select(x => x.Message);

        foreach (var parameters in parametersCollection)
        {
            var group = new Group { Id = parameters.Id, Name = parameters.Name, UsedMessenger = parameters.UsedMessenger };

            await dbContext.Set<Group>().AddAsync(group);
            await dbContext.SaveChangesAsync();
        }
    }
}