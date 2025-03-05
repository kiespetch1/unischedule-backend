using System.Reflection;
using UniSchedule.Abstractions.Commands;
using UniSchedule.Events.Shared.Parameters;
using UniSchedule.Events.Shared.Publishers;
using UniSchedule.Extensions.DI.Auth;
using UniSchedule.Extensions.DI.Configuration;
using UniSchedule.Extensions.DI.Controllers;
using UniSchedule.Extensions.DI.Database;
using UniSchedule.Extensions.DI.Messaging;
using UniSchedule.Extensions.DI.Messaging.Settings;
using UniSchedule.Extensions.DI.Middleware;
using UniSchedule.Extensions.DI.Settings.ApiDocumentation;
using UniSchedule.Extensions.DI.Settings.Auth;
using UniSchedule.Extensions.DI.Swagger;
using UniSchedule.Extensions.Utils;
using UniSchedule.Identity.Database;
using UniSchedule.Identity.Database.Helpers;
using UniSchedule.Identity.Services;
using UniSchedule.Validation;

namespace UniSchedule.Identity.Api;

public class Startup(IConfiguration configuration)
{
    private readonly ApiDocumentationSettings _apiDocsSettings = configuration.GetSectionAs<ApiDocumentationSettings>();

    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDatabase<DatabaseContext>(connectionString!);
        var rabbitMqSettings = configuration.GetSectionAs<RabbitMqSettings>();
        services.AddDataSeeder<DataSeeder, DatabaseContext>();
        services.AddRabbitMq(rabbitMqSettings, configure =>
        {
            configure.AddPublisher<EventsPublisher, EventCreateParameters>();
        }, messageConfigure =>
        {
            messageConfigure.MessageConfigure<EventCreateParameters>();
        });

        services.AddDomainServices();
        services.AddValidation();
        services.AddAuthorization();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddControllersWithSnakeCase();
        services.AddApiDocumentation(_apiDocsSettings);

        var authSettings = configuration.GetSectionAs<JwtTokenSettings>();
        services.AddAuthConfiguration(authSettings);
        services.AddUserContextProvider(); 
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!EnvironmentUtils.IsProduction)
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseErrorHandler();

        app.UseApiDocumentation(_apiDocsSettings);

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapApiDocumentation(_apiDocsSettings);
        });
    }
}