using Microsoft.Extensions.DependencyInjection;
using UniSchedule.Abstractions.Commands;

namespace UniSchedule.Identity.Commands;

/// <summary>
///     Методы расширения для DI
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddCommands<UserCommands>();

        return services;
    }
}