namespace UniSchedule.Extensions.DI.Settings.ApiDocumentation;

/// <summary>
///     Настройки для документирования API
/// </summary>
public class ApiDocumentationSettings
{
    /// <summary>
    ///     Настройки Swagger
    /// </summary>
    public required SwaggerSettings Swagger { get; set; }
}