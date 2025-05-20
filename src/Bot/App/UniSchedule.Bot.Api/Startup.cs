using UniSchedule.Extensions.DI.Auth;
using UniSchedule.Extensions.DI.Configuration;
using UniSchedule.Extensions.DI.Controllers;
using UniSchedule.Extensions.DI.Messaging.Settings;
using UniSchedule.Extensions.DI.Middleware;
using UniSchedule.Extensions.DI.Settings.ApiDocumentation;
using UniSchedule.Extensions.DI.Swagger;
using UniSchedule.Extensions.Utils;
using UniSchedule.Messaging;
using UniSchedule.Validation;
using UniSсhedule.Bot.Shared.Announcements;
using UniSсhedule.Bot.Shared.Publishers;

namespace UniSchedule.Bot.Api;

public class Startup(IConfiguration configuration)
{
    private readonly ApiDocumentationSettings _apiDocsSettings = configuration.GetSectionAs<ApiDocumentationSettings>();

    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureForwardedHeaders();

        var rabbitMqSettings = configuration.GetSectionAs<RabbitMqSettings>();
        services.AddRabbitMq(rabbitMqSettings, configure =>
        {
            configure.AddPublisher<AnnouncementsCreatedPublisher, AnnouncementMqCreateParameters>();
        }, messageConfigure =>
        {
            messageConfigure.MessageConfigure<AnnouncementMqCreateParameters>();
        });

        services.AddValidation();
        services.AddAuthorization();
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddControllersWithSnakeCase();
        services.AddApiDocumentation(_apiDocsSettings);
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

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapApiDocumentation(_apiDocsSettings);
        });
    }
}