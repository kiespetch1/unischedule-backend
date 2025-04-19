using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using UniSchedule.Extensions.DI.Settings.ApiDocumentation;
using UniSchedule.Extensions.Utils;

namespace UniSchedule.Extensions.DI.Swagger;

/// <summary>
///     Методы расширения DI для документирования API
/// </summary>
public static class ApiDocumentationExtensions
{
    /// <summary>
    ///     Добавление средств документирования API в DI
    /// </summary>
    /// <param name="services">
    ///     <see cref="IServiceCollection" />
    /// </param>
    /// <param name="settings">Настройки</param>
    /// <returns>
    ///     <see cref="IServiceCollection" />
    /// </returns>
    public static IServiceCollection AddApiDocumentation(
        this IServiceCollection services,
        ApiDocumentationSettings settings)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(settings.Swagger.Version,
                new OpenApiInfo
                {
                    Version = settings.Swagger.Version,
                    Title = settings.Swagger.Title,
                    Description = settings.Swagger.Description
                });
            c.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    Description = "Авторизация с помощью JWT токена",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    new List<string>()
                }
            });

            var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
            foreach (var xmlFile in xmlFiles)
            {
                c.IncludeXmlComments(xmlFile);
            }

            c.OperationFilter<SnakeCaseOperationFilter>();
            c.OperationFilter<StatusCodeFilter>();
            c.OperationFilter<XsrfOperationFilter>();
        });
        services.AddSwaggerGenNewtonsoftSupport();

        return services;
    }

    /// <summary>
    ///     Подключение документации API
    /// </summary>
    /// <param name="app">
    ///     <see cref="IApplicationBuilder" />
    /// </param>
    /// <param name="settings">Настройки</param>
    /// <returns>
    ///     <see cref="IApplicationBuilder" />
    /// </returns>
    public static IApplicationBuilder UseApiDocumentation(
        this IApplicationBuilder app,
        ApiDocumentationSettings settings)
    {
        app.UseSwaggerOptions(settings.Swagger);

        return app;
    }

    /// <summary>
    ///     Переадресация для документации API
    /// </summary>
    /// <param name="endpoints">
    ///     <see cref="IEndpointRouteBuilder" />
    /// </param>
    /// <param name="settings">Настройки документации API</param>
    /// <returns>
    ///     <see cref="IEndpointRouteBuilder" />
    /// </returns>
    public static IEndpointRouteBuilder MapApiDocumentation(this IEndpointRouteBuilder endpoints,
        ApiDocumentationSettings settings)
    {
        if (!EnvironmentUtils.IsProduction)
        {
            endpoints.MapGet("/", settings.Scalar.RoutePrefix);

            endpoints.MapScalarApiReference(settings.Scalar.RoutePrefix, o =>
            {
                o.Title = settings.Scalar.DocumentTitle;
                o.Theme = ScalarTheme.Default;
                o.OpenApiRoutePattern = settings.Swagger.RouteTemplate;
            });
        }

        return endpoints;
    }

    /// <summary>
    ///     Подключение Swagger
    /// </summary>
    /// <param name="app">
    ///     <see cref="IApplicationBuilder" />
    /// </param>
    /// <param name="settings">Настройки Swagger</param>
    /// <returns>
    ///     <see cref="IApplicationBuilder" />
    /// </returns>
    private static IApplicationBuilder UseSwaggerOptions(
        this IApplicationBuilder app,
        SwaggerSettings settings)
    {
        app.UseSwagger(c =>
        {
            c.RouteTemplate = settings.RouteTemplate;
        });
        app.UseSwaggerUI(c =>
        {
            c.DocumentTitle = settings.DocumentTitle;
            c.RoutePrefix = settings.RoutePrefix;
        });

        return app;
    }

    /// <summary>
    ///     Переадресация для GET-запросов
    /// </summary>
    /// <param name="endpoints">
    ///     <see cref="IEndpointRouteBuilder" />
    /// </param>
    /// <param name="from">Исходный route</param>
    /// <param name="to">Целевой route</param>
    /// <returns>
    ///     <see cref="IEndpointRouteBuilder" />
    /// </returns>
    private static IEndpointRouteBuilder MapGet(this IEndpointRouteBuilder endpoints, string from, string to)
    {
        endpoints.MapGet(from, http =>
        {
            http.Response.Redirect(to);
            return Task.CompletedTask;
        });

        return endpoints;
    }
}