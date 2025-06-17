using AngleSharp;
using Microsoft.Extensions.DependencyInjection;
using UniSchedule.Schedule.Services.Abstractions;

namespace UniSchedule.Schedule.Services;

/// <summary>
///     Методы расширения для DI
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Добавление сервисов в DI
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Обновленная коллекция сервисов</returns>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IClassService, ClassService>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<IPreferencesService, PreferencesService>();
        services.AddScoped<ILmsDataService, LmsDataService>();

        return services;
    }

    /// <summary>
    ///     Добавление AngleSharp
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    public static IServiceCollection AddAngleSharp(this IServiceCollection services)
    {
        services.AddSingleton<IConfiguration>(_ => Configuration.Default.WithDefaultLoader());
        services.AddSingleton<IBrowsingContext>
            (sp => BrowsingContext.New(sp.GetRequiredService<IConfiguration>()));

        return services;
    }
}