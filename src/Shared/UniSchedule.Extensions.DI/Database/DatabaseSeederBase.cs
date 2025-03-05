using Microsoft.EntityFrameworkCore;
using UniSchedule.Abstractions.Helpers.Database;

namespace UniSchedule.Extensions.DI.Database;


/// <inheritdoc cref="IDataSeeder" />
public abstract class DatabaseSeederBase<TContext>(TContext context) : IDataSeeder
    where TContext : DbContext
{
    /// <summary>
    ///     Контекст базы данных
    /// </summary>
    protected readonly TContext context = context;

    /// <inheritdoc cref="IDataSeeder.SeedAsync" />
    public abstract Task SeedAsync();
}