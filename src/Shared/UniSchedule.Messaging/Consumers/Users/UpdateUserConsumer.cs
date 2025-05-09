using MassTransit;
using UniSchedule.Entities;
using UniSchedule.Extensions.Collections;
using UniSchedule.Identity.DTO.Messages.Users;

namespace UniSchedule.Messaging.Consumers.Users;

/// <summary>
///     Консьюмер для обновления пользователя
/// </summary>
public class UpdateUserConsumer(IDbContextAccessor dbContextAccessor)
    : IConsumer<Batch<UserMqUpdateParameters>>
{
    /// <inheritdoc />
    public async Task Consume(ConsumeContext<Batch<UserMqUpdateParameters>> context)
    {
        var dbContext = dbContextAccessor.GetDbContext();
        var parametersCollection = context.Message.Select(x => x.Message);

        foreach (var parameters in parametersCollection)
        {
            var user = await dbContext.Set<User>().SingleOrNotFoundAsync(x => x.Id == parameters.Id);

            user.Surname = parameters.Surname;
            user.Name = parameters.Name;
            user.Patronymic = parameters.Patronymic;
            user.Email = parameters.Email;

            await dbContext.SaveChangesAsync();
        }
    }
}