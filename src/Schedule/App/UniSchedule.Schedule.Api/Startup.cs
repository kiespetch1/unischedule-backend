using System.Reflection;
using UniSchedule.Events.Shared.Parameters;
using UniSchedule.Events.Shared.Publishers;
using UniSchedule.Extensions.DI.Auth;
using UniSchedule.Extensions.DI.Configuration;
using UniSchedule.Extensions.DI.Controllers;
using UniSchedule.Extensions.DI.CORS;
using UniSchedule.Extensions.DI.Database;
using UniSchedule.Extensions.DI.Messaging.Settings;
using UniSchedule.Extensions.DI.Middleware;
using UniSchedule.Extensions.DI.Settings.ApiDocumentation;
using UniSchedule.Extensions.DI.Settings.Auth;
using UniSchedule.Extensions.DI.Swagger;
using UniSchedule.Extensions.Utils;
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
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDatabase<DatabaseContext>(connectionString!);
        services.AddScoped<IDbContextAccessor, DbContextAccessor<DatabaseContext>>();
        var rabbitMqSettings = configuration.GetSectionAs<RabbitMqSettings>();
        services.AddRabbitMq(rabbitMqSettings, configure =>
        {
            configure.AddPublisher<EventsPublisher, EventCreateParameters>();
            configure.AddUsersConsumers();
        }, messageConfigure =>
        {
            messageConfigure.MessageConfigure<EventCreateParameters>();
        });

        services.AddCommands();
        services.AddQueries();
        services.AddServices();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        services.AddValidation();
        services.AddAuthorization();
        services.AddAntiforgeryWithOptions();
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