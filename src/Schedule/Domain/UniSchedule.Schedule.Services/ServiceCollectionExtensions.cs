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

        return services;
    }
}