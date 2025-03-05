using Microsoft.EntityFrameworkCore;
using UniSchedule.Identity.Entities;

namespace UniSchedule.Extensions.DI.Database;

/// <inheritdoc cref="DatabaseSeederBase{TContext}"/>
public abstract class HandbookSeederBase<TContext>(TContext context)
    : DatabaseSeederBase<TContext>(context) where TContext : DbContext
{
    /// <summary>
    ///     Инициализация справочника
    /// </summary>
    /// <typeparam name="THandbookOption">Тип enum</typeparam>
    /// <typeparam name="THandbookEntity">Тип перечисления</typeparam>
    protected async Task SeedHandbookEntryAsync<THandbookOption, THandbookEntity>()
        where THandbookOption : struct, Enum
        where THandbookEntity : HandbookEntry<THandbookOption>
    {
        var existingEntityIds = await context.Set<THandbookEntity>()
            .Select(a => a.Id)
            .ToListAsync();
        var entities = CreateHandbookEntry<THandbookOption, THandbookEntity>().ToList();

        var entitiesToCreate = entities.Where(a => !existingEntityIds.Contains(a.Id));
        var entitiesToUpdate = entities.Where(a => existingEntityIds.Contains(a.Id));

        await context.Set<THandbookEntity>().AddRangeAsync(entitiesToCreate);
        context.Set<THandbookEntity>().UpdateRange(entitiesToUpdate);
    }

    /// <summary>
    ///     Создание сокращённого справочника на основе Enum
    /// </summary>
    /// <typeparam name="THandbookOption">Тип enum</typeparam>
    /// <typeparam name="THandbookEntity">Тип перечисления</typeparam>
    /// <returns>Справочник в формате <typeparamref name="THandbookEntity" /></returns>
    /// <exception cref="InvalidOperationException">Ошибка при создании экземпляра справочника</exception>
    private static List<THandbookEntity> CreateHandbookEntry<THandbookOption, THandbookEntity>()
        where THandbookOption : struct, Enum
        where THandbookEntity : HandbookEntry<THandbookOption>
    {
        var result = new List<THandbookEntity>();
        var enumValues = Enum.GetValues<THandbookOption>();

        foreach (var enumValue in enumValues)
        {
            var entity = (THandbookEntity)Activator.CreateInstance(typeof(THandbookEntity), enumValue)!
                         ?? throw new InvalidOperationException(
                             $"Ошибка в создании экземпляра типа {typeof(THandbookEntity)}");

            result.Add(entity);
        }

        return result;
    }
}