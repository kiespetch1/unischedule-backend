using Microsoft.Extensions.DependencyInjection;
using UniSchedule.Abstractions.Commands;

namespace UniSchedule.Schedule.Commands;

/// <summary>
///     Методы расширения для DI
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddCommands<WeekCommands>();

        return services;
    }
}