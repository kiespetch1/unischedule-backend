﻿using System.Reflection;
using UniSchedule.Events.Commands;
using UniSchedule.Events.Commands.Consumers;
using UniSchedule.Events.Database;
using UniSchedule.Events.Database.Helpers;
using UniSchedule.Extensions.DI.Auth;
using UniSchedule.Extensions.DI.Configuration;
using UniSchedule.Extensions.DI.Controllers;
using UniSchedule.Extensions.DI.CORS;
using UniSchedule.Extensions.DI.Database;
using UniSchedule.Extensions.DI.Messaging.Settings;
using UniSchedule.Extensions.DI.Middleware;
using UniSchedule.Extensions.DI.Monitoring;
using UniSchedule.Extensions.DI.Settings.ApiDocumentation;
using UniSchedule.Extensions.DI.Settings.Auth;
using UniSchedule.Extensions.DI.Swagger;
using UniSchedule.Extensions.Utils;
using UniSchedule.Identity.Entities.Settings;
using UniSchedule.Messaging;
using UniSchedule.Validation;

namespace UniSchedule.Events.Api;

public class Startup(IConfiguration configuration)
{
    private readonly ApiDocumentationSettings _apiDocsSettings = configuration.GetSectionAs<ApiDocumentationSettings>();

    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureForwardedHeaders();
        
        var cookieSettings = configuration.GetSectionAs<CookieSettings>();
        services.AddSingleton(cookieSettings);
        
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("DefaultConnection is missing");
        services.AddDatabase<DatabaseContext>(connectionString!);
        var rabbitMqSettings = configuration.GetSectionAs<RabbitMqSettings>();
        services.AddRabbitMq(rabbitMqSettings, x =>
            x.AddBatchConsumer<EventsConsumer, EventsConsumerDefinition>());

        services.AddCommands();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        services.AddDataSeeder<DataSeeder, DatabaseContext>();
        services.AddValidation();
        services.AddAuthorization();
        services.AddAntiforgeryWithOptions(cookieSettings);
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddControllersWithSnakeCase();
        services.AddApiDocumentation(_apiDocsSettings);
        services.AddCors(EnvironmentUtils.DefaultId.ToString());

        var authSettings = configuration.GetSectionAs<JwtTokenSettings>();
        services.AddAuthConfiguration(authSettings);
        services.AddUserContextProvider();

        services.AddServiceMonitoring();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseForwardedHeaders();
        if (!EnvironmentUtils.IsProduction)
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseErrorHandler();
        app.UseGlobalCookiePolicy();

        app.UseApiDocumentation(_apiDocsSettings);

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors(EnvironmentUtils.DefaultId.ToString());
        app.UseOpenTelemetryPrometheusScrapingEndpoint();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseXsrfProtection();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapApiDocumentation(_apiDocsSettings);
        });
    }
}