using Humanizer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace UniSchedule.Extensions.DI.Swagger;

/// <summary>
///     Фильтрация операций в Swagger
/// </summary>
public class SnakeCaseOperationFilter : IOperationFilter
{
    /// <summary />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var parameter in operation.Parameters)
        {
            parameter.Name = parameter.Name.Underscore();
        }
    }
}