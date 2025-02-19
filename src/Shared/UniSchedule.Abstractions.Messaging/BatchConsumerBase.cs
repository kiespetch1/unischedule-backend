using FluentValidation;
using MassTransit;
using UniSchedule.Abstractions.Commands;
using UniSchedule.Abstractions.Entities;

namespace UniSchedule.Abstractions.Messaging;

/// <summary>
///     Базовый класс для обработки сообщений
/// </summary>
/// <param name="command">Команда создания сущности</param>
/// <param name="validator">Валидатор для параметров создания сущности</param>
/// <typeparam name="TEntity">Тип сущности, должен наследоваться от <see cref="Entity{TKey}" /></typeparam>
/// <typeparam name="TParams">Тип паарметров для создания сущности</typeparam>
/// <typeparam name="TKey">Тип ключа сущности</typeparam>
public abstract class BatchConsumerBase<TEntity, TParams, TKey>(
    ICreateCommand<TEntity, IEnumerable<TParams>, TKey> command,
    IValidator<TParams> validator) : IConsumer<Batch<TParams>>
    where TEntity : Entity<TKey>
    where TParams : class
{
    /// <inheritdoc />
    public async Task Consume(ConsumeContext<Batch<TParams>> context)
    {
        var validationTasks = context.Message
            .Select(x => validator.ValidateAndThrowAsync(x.Message));
        await Task.WhenAll(validationTasks);

        await command.ExecuteAsync(context.Message.Select(x => x.Message));
    }
}