using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Extensions.DI.Settings.Auth;

namespace UniSchedule.Extensions.DI.Auth;

/// <summary>
///     Методы расширения для авторизации
/// </summary>
public static class AuthExtensions
{
    /// <summary>
    ///     Добавляет авторизации в DI
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <param name="settings">Настройки авторизации</param>
    /// <returns>Коллекция сервисов</returns>
    public static IServiceCollection AddAuthConfiguration(this IServiceCollection services, JwtTokenSettings settings)
    {
        services.AddSingleton(settings);
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecurityKey)),
                    ValidateIssuer = true,
                    ValidIssuer = settings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = settings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        services.AddAuthorization();

        return services;
    }

    /// <summary>
    ///     Добавление провайдера пользователей в DI
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов</returns>
    public static IServiceCollection AddUserContextProvider(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddTransient<IUserContextProvider, UserContextProvider>();

        return services;
    }
}