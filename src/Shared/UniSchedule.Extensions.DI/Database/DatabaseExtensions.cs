using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UniSchedule.Abstractions.Helpers.Database;
using UniSchedule.Extensions.Helpers;

namespace UniSchedule.Extensions.DI.Database;

public static class DatabaseExtensions
{
    /// <summary>
    ///     Добавление базы данных в DI
    /// </summary>
    /// <param name="services">Коллекция сервисов <see cref="IServiceCollection" /></param>
    /// <param name="connectionString">Строка подключения к базе данных</param>
    /// <returns>Коллекция сервисов <see cref="IServiceCollection" /></returns>
    public static IServiceCollection AddDatabase<TContext>(this IServiceCollection services, string connectionString)
        where TContext : DbContext, IMigrationDatabase
    {
        services.AddDbContext<TContext>(options =>
        {
            options.UseNpgsql(connectionString, config =>
            {
                config.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
        });
        services.AddTransient<IMigrationDatabase, TContext>();

        return services;
    }
    
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
    
    /// <summary>
    ///     Добавление инициализатора данных
    /// </summary>
    /// <param name="services">Коллекция сервисов <see cref="IServiceCollection" /></param>
    /// <typeparam name="TSeeder">Инициализатор данных</typeparam>
    /// <typeparam name="TContext">Контекст базы данных</typeparam>
    /// <returns>Коллекция сервисов <see cref="IServiceCollection" /></returns>
    public static IServiceCollection AddDataSeeder<TSeeder, TContext>(this IServiceCollection services)
        where TSeeder : DatabaseSeederBase<TContext> where TContext : DbContext
    {
        services.AddScoped<IDataSeeder, TSeeder>();

        return services;
    }

    /// <summary>
    ///     Инициализация данных
    /// </summary>
    /// <param name="host">Хост</param>
    public static async Task<IHost> SeedDataAsync(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var dataSeeder = services.GetRequiredService<IDataSeeder>();
            await dataSeeder.SeedAsync();
        }

        return host;
    }

}