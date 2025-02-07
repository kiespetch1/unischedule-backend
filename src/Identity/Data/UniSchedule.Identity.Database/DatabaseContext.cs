using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UniSchedule.Extensions.Helpers;

namespace UniSchedule.Identity.Database;

/// <summary>
///     Контекст базы данных
/// </summary>
public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options), IMigrationDatabase
{
    public async Task MigrateAsync()
    {
        await Database.MigrateAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}