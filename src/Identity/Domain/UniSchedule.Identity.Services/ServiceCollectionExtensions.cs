using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Identity.Entities;
using UniSchedule.Identity.Services.Abstractions;
using UniSchedule.Identity.Services.Providers;

namespace UniSchedule.Identity.Services;

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
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddTransient<ITokenContextProvider, TokenContextProvider>();
        services.AddTransient<ITokenProvider, JwtTokenProvider>();
        services.AddTransient<IUserContextProvider, UserContextProvider>();

        services.AddScoped<IAuthService, AuthService>();

        return services;
    }

 
}