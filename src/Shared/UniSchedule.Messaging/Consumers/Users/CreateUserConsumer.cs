using MassTransit;
using UniSchedule.Entities;
using UniSchedule.Identity.DTO.Messages.Users;

namespace UniSchedule.Messaging.Consumers.Users;

/// <summary>
///     Консьюмер для создания пользователей
/// </summary>
public class CreateUserConsumer(IDbContextAccessor dbContextAccessor)
    : IConsumer<Batch<UserMqCreateParameters>>
{
    /// <inheritdoc />
    public async Task Consume(ConsumeContext<Batch<UserMqCreateParameters>> context)
    {
        var dbContext = dbContextAccessor.GetDbContext();
        var parametersCollection = context.Message.Select(x => x.Message);

        foreach (var parameters in parametersCollection)
        {
            var entity = new User
            {
                Id = parameters.Id,
                Surname = parameters.Surname,
                Name = parameters.Name,
                Patronymic = parameters.Patronymic,
                Email = parameters.Email
            };

            await dbContext.Set<User>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}