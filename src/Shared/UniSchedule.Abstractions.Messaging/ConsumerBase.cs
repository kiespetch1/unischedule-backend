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
/// <typeparam name="TParams">Тип параметров для создания сущности</typeparam>
/// <typeparam name="TKey">Тип ключа сущности</typeparam>
public abstract class ConsumerBase<TEntity, TParams, TKey>(
    ICreateCommand<TEntity, TParams, TKey> command,
    IValidator<TParams> validator) : IConsumer<TParams>
    where TEntity : Entity<TKey>
    where TParams : class
{
    /// <inheritdoc />
    public async Task Consume(ConsumeContext<TParams> context)
    {
        await validator.ValidateAndThrowAsync(context.Message);
        await command.ExecuteAsync(context.Message);
    }
}