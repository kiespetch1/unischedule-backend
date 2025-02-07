using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UniSchedule.Extensions.Helpers;

namespace UniSchedule.Extensions.DI.Database;

public static class DatabaseExtensions
{
    /// <summary>
    ///     Миграции БД
    /// </summary>
    /// <param name="host">Хост</param>
    public static async Task<IHost> MigrateDatabaseAsync(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var db = services.GetRequiredService<IMigrationDatabase>();
            await db.MigrateAsync();
        }

        return host;
    }
}