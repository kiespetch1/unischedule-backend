using MassTransit;
using UniSchedule.Entities;
using UniSchedule.Extensions.Collections;
using UniSchedule.Identity.DTO.Messages.Groups;
using UniSchedule.Identity.Entities;

namespace UniSchedule.Messaging.Consumers.Groups;

/// <summary>
///     Консьюмер для удаления группы
/// </summary>
public class DeleteGroupConsumer(IDbContextAccessor dbContextAccessor) : IConsumer<Batch<GroupMqDeleteParameters>>
{
    /// <inheritdoc />
    public async Task Consume(ConsumeContext<Batch<GroupMqDeleteParameters>> context)
    {
        var dbContext = dbContextAccessor.GetDbContext();
        var parametersCollection = context.Message.Select(x => x.Message);

        foreach (var parameters in parametersCollection)
        {
            var group = await dbContext.Set<Group>().SingleOrNotFoundAsync(parameters.Id);
            dbContext.Set<Group>().Remove(group);

            await dbContext.SaveChangesAsync();
        }
    }
}