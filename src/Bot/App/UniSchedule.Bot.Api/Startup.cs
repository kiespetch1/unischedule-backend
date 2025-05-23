using UniSchedule.Bot.Database;
using UniSchedule.Bot.Entities.Settings;
using UniSchedule.Bot.Services;
using UniSchedule.Extensions.DI.Auth;
using UniSchedule.Extensions.DI.CORS;
using UniSchedule.Extensions.DI.Configuration;
using UniSchedule.Extensions.DI.Controllers;
using UniSchedule.Extensions.DI.Database;
using UniSchedule.Extensions.DI.Messaging.Settings;
using UniSchedule.Extensions.DI.Middleware;
using UniSchedule.Extensions.DI.Settings.ApiDocumentation;
using UniSchedule.Extensions.DI.Settings.Auth;
using UniSchedule.Extensions.DI.Swagger;
using UniSchedule.Extensions.Utils;
using UniSchedule.Identity.Entities.Settings;
using UniSchedule.Messaging;
using UniSchedule.Validation;
using UniSchedule.Bot.Shared.Announcements;
using UniSchedule.Bot.Shared.Publishers;

namespace UniSchedule.Bot.Api;

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
        var rabbitMqSettings = configuration.GetSectionAs<RabbitMqSettings>();
        services.AddRabbitMq(rabbitMqSettings, configure =>
        {
            configure.AddPublisher<AnnouncementsCreatedPublisher, AnnouncementMqCreateParameters>();
            configure.AddGroupsConsumers();
        }, messageConfigure =>
        {
            messageConfigure.MessageConfigure<AnnouncementMqCreateParameters>();
        });

        var vkApiSettings = configuration.GetSectionAs<VkApiSettings>();
        services.AddSingleton(vkApiSettings);
        services.AddVkClient(vkApiSettings);

        services.AddServices();

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