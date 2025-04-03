using Microsoft.Extensions.DependencyInjection;
using UniSchedule.Abstractions.Helpers;

namespace UniSchedule.Extensions.DI.Sync;

/// <summary>
///     Методы расширения для DI
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Добавление сервисов в DI для синхронизации данных между микросервисами
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов</returns>
    public static IServiceCollection AddSyncData<TService>(this IServiceCollection services)
        where TService : class, ISyncService
    {
        services.AddScoped<ISyncService, TService>();

        return services;
    }
}