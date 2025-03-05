using UniSchedule.Events.Entities;
using UniSchedule.Extensions.DI.Database;
using UniSchedule.Identity.Shared;
using Action = UniSchedule.Events.Entities.Action;

namespace UniSchedule.Events.Database.Helpers;

/// <inheritdoc cref="HandbookSeederBase{TContext}" />
public class DataSeeder(DatabaseContext context) : HandbookSeederBase<DatabaseContext>(context)
{
    /// <inheritdoc cref="HandbookSeederBase{TContext}.SeedAsync" />
    public override async Task SeedAsync()
    {
        await SeedHandbooksAsync();
    }

    #region [ Handbooks ]

    /// <summary>
    ///     Инициализация справочников
    /// </summary>
    private async Task SeedHandbooksAsync()
    {
        await SeedHandbookEntryAsync<ActionOption, Action>();
        await SeedHandbookEntryAsync<SubjectOption, Subject>();

        await context.SaveChangesAsync();
    }

    #endregion
}