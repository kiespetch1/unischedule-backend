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
using UniSchedule.Extensions.DI.Sync;
using UniSchedule.Extensions.Utils;
using UniSchedule.Identity.Database;
using UniSchedule.Identity.Database.Helpers;
using UniSchedule.Identity.DTO.Messages;
using UniSchedule.Identity.Services;
using UniSchedule.Identity.Services.Publishers;
using UniSchedule.Messaging;
using UniSchedule.Validation;

namespace UniSchedule.Identity.Api;

public class Startup(IConfiguration configuration)
{
    private readonly ApiDocumentationSettings _apiDocsSettings = configuration.GetSectionAs<ApiDocumentationSettings>();

    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDatabase<DatabaseContext>(connectionString!);
        services.AddScoped<IDbContextAccessor, DbContextAccessor<DatabaseContext>>();
        var rabbitMqSettings = configuration.GetSectionAs<RabbitMqSettings>();
        var credentialsSettings = configuration.GetSectionAs<AdminCredentials>();
        services.AddSingleton(credentialsSettings);
        services.AddDataSeeder<DataSeeder, DatabaseContext>();
        services.AddSyncData<UsersSyncService>();
        services.AddRabbitMq(rabbitMqSettings, configure =>
        {
            configure.AddPublisher<EventsPublisher, EventCreateParameters>();
            configure.AddPublisher<UserCreatedPublisher, UserMqCreateParameters>();
            configure.AddPublisher<UserUpdatedPublisher, UserMqUpdateParameters>();
            configure.AddPublisher<UserDeletedPublisher, UserMqDeleteParameters>();
            configure.AddPublisher<UsersSyncPublisher, UsersMqSyncParameters>();
        }, messageConfigure =>
        {
            messageConfigure.MessageConfigure<EventCreateParameters>();
            messageConfigure.MessageConfigure<UserMqCreateParameters>();
            messageConfigure.MessageConfigure<UserMqUpdateParameters>();
            messageConfigure.MessageConfigure<UserMqDeleteParameters>();
            messageConfigure.MessageConfigure<UsersMqSyncParameters>();
        });

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddDomainServices();
        
        services.AddValidation();
        services.AddAuthorization();
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

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapApiDocumentation(_apiDocsSettings);
        });
    }
}