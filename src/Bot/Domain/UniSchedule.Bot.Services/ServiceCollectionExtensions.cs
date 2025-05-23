using Microsoft.Extensions.DependencyInjection;
using UniSchedule.Bot.Entities.Settings;
using UniSchedule.Bot.Services.Abstractions;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model;

namespace UniSchedule.Bot.Services;

/// <summary>
///     Методы расширения для DI
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Добавление сервисов в DI
    /// </summary>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IEventService, EventService>();

        return services;
    }

    /// <summary>
    ///     Добавление VkNet в DI
    /// </summary>
    public static IServiceCollection AddVkClient(this IServiceCollection services, VkApiSettings settings)
    {
        services.AddSingleton<IVkApi>(_ =>
        {
            var api = new VkApi();
            api.Authorize(new ApiAuthParams
            {
                AccessToken = settings.AccessToken,
                ClientSecret = settings.Secret,
                IsTokenUpdateAutomatically = false
            });
            return api;
        });

        return services;
    }
}