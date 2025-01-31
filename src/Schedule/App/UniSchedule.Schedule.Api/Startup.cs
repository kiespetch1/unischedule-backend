using UniSchedule.Extensions.DI.Configuration;
using UniSchedule.Extensions.DI.Controllers;
using UniSchedule.Extensions.DI.Settings.ApiDocumentation;
using UniSchedule.Extensions.DI.Swagger;

namespace UniSchedule.Api;

public class Startup(IConfiguration configuration)
{
    private readonly ApiDocumentationSettings _apiDocsSettings = configuration.GetSectionAs<ApiDocumentationSettings>();
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddControllersWithSnakeCase();
        services.AddApiDocumentation(_apiDocsSettings);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        
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