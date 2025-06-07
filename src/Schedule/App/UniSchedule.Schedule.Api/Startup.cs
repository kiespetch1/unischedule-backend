using System.Reflection;
using UniSchedule.Bot.Shared.Announcements;
using UniSchedule.Events.Shared.Parameters;
using UniSchedule.Events.Shared.Publishers;
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
using UniSchedule.Extensions.DI.Sync;
using UniSchedule.Extensions.Utils;
using UniSchedule.Identity.DTO.Messages.Groups;
using UniSchedule.Identity.Entities.Settings;
using UniSchedule.Identity.Services.Publishers.Groups;
using UniSchedule.Messaging;
using UniSchedule.Schedule.Commands;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Queries;
using UniSchedule.Schedule.Services;
using UniSchedule.Validation;

namespace UniSchedule.Schedule.Api;

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
        services.AddDatabase<DatabaseContext>(connectionString);
        services.AddScoped<IDbContextAccessor, DbContextAccessor<DatabaseContext>>();
        services.AddSyncData<GroupsSyncService>();
        var rabbitMqSettings = configuration.GetSectionAs<RabbitMqSettings>();
        services.AddDataSeeder<DataSeeder, DatabaseContext>();
        services.AddRabbitMq(rabbitMqSettings, configure =>
        {
            configure.AddPublisher<EventsPublisher, EventCreateParameters>();
            configure.AddPublisher<GroupCreatedPublisher, GroupMqCreateParameters>();
            configure.AddPublisher<GroupDeletedPublisher, GroupMqDeleteParameters>();
            configure.AddPublisher<GroupsSyncPublisher, GroupsMqSyncParameters>();
            configure.AddAnnouncementsConsumers();
            configure.AddUsersConsumers();
        }, messageConfigure =>
        {
            messageConfigure.MessageConfigure<EventCreateParameters>();
            messageConfigure.MessageConfigure<GroupMqCreateParameters>();
            messageConfigure.MessageConfigure<GroupMqDeleteParameters>();
            messageConfigure.MessageConfigure<GroupsMqSyncParameters>();
            messageConfigure.MessageConfigure<AnnouncementMqCreateParameters>();
        });

        services.AddCommands();
        services.AddQueries();
        services.AddServices();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddAngleSharp();

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