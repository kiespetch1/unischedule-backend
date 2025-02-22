using Microsoft.Extensions.DependencyInjection;
using UniSchedule.Abstractions.Commands;
using UniSchedule.Events.Commands.Events;

namespace UniSchedule.Events.Commands;

/// <summary>
///     Методы расширения для DI
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Добавление Command-слоя в DI
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Обновленная коллекция сервисов</returns>
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddCommands<EventCommands>();

        return services;
    }
}