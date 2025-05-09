using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Extensions.DI.Settings.Auth;
using IPNetwork = Microsoft.AspNetCore.HttpOverrides.IPNetwork;

namespace UniSchedule.Extensions.DI.Auth;

/// <summary>
///     Методы расширения для авторизации
/// </summary>
public static class AuthExtensions
{
    /// <summary>
    ///     Добавляет авторизацию в DI
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
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.ContainsKey("x-token"))
                        {
                            context.Token = context.Request.Cookies["x-token"];
                        }

                        return Task.CompletedTask;
                    }
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

    /// <summary>
    ///     Задание конфигурации для корректной переадресации XSRF-заголовков запросов внутри Docker-сети
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов</returns>
    public static IServiceCollection ConfigureForwardedHeaders(this IServiceCollection services)
    {
        const string dockerNetworkIp = "172.18.0.0";
        const int dockerNetworkSubnet = 16;
        services.Configure<ForwardedHeadersOptions>(opts =>
        {
            opts.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            opts.KnownNetworks.Add(new IPNetwork(IPAddress.Parse(dockerNetworkIp), dockerNetworkSubnet));
            opts.RequireHeaderSymmetry = false;
        });

        return services;
    }

    public static IServiceCollection AddAntiforgeryWithOptions(this IServiceCollection services)
    {
        services.AddDataProtection()
            .SetApplicationName("UniSchedule")
            .PersistKeysToFileSystem(new DirectoryInfo("/app/keys"));

        services.AddAntiforgery(options =>
        {
            options.Cookie.Name = "XSRF-COOKIE";
            options.Cookie.Domain = "localhost";
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.HttpOnly = false;
            options.HeaderName = "XSRF-TOKEN";
        });

        return services;
    }

    /// <summary>
    ///     Установка настроек безопасности для кук
    /// </summary>
    public static IApplicationBuilder UseGlobalCookiePolicy(this IApplicationBuilder builder)
    {
        builder.UseCookiePolicy(new CookiePolicyOptions
        {
            MinimumSameSitePolicy = SameSiteMode.None, Secure = CookieSecurePolicy.Always
        });
        return builder;
    }
}