using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UniSchedule.Extensions.Helpers;

namespace UniSchedule.Extensions.DI.Database;

public static class ServiceCollectionExtensions
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
}