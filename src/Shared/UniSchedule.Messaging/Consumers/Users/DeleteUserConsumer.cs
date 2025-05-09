using MassTransit;
using UniSchedule.Entities;
using UniSchedule.Extensions.Collections;
using UniSchedule.Identity.DTO.Messages.Users;

namespace UniSchedule.Messaging.Consumers.Users;

/// <summary>
///     Консьюмер для удаления пользователя
/// </summary>
public class DeleteUserConsumer(IDbContextAccessor dbContextAccessor)
    : IConsumer<Batch<UserMqDeleteParameters>>
{
    /// <inheritdoc />
    public async Task Consume(ConsumeContext<Batch<UserMqDeleteParameters>> context)
    {
        var dbContext = dbContextAccessor.GetDbContext();
        var parametersCollection = context.Message.Select(x => x.Message);

        foreach (var parameters in parametersCollection)
        {
            var user = await dbContext.Set<User>().SingleOrNotFoundAsync(parameters.Id);
            dbContext.Set<User>().Remove(user);

            await dbContext.SaveChangesAsync();
        }
    }
}