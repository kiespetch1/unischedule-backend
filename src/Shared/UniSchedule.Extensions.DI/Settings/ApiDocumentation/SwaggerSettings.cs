namespace UniSchedule.Extensions.DI.Settings.ApiDocumentation;

/// <summary>
///     Настройки Swagger
/// </summary>
public class SwaggerSettings
{
    /// <summary>
    ///     Версия API
    /// </summary>
    public required string Version { get; set; }

    /// <summary>
    ///     Наименование API
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    ///     Наименование страницы Swagger
    /// </summary>
    public required string DocumentTitle { get; set; }

    /// <summary>
    ///     Описание
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    ///     Путь к файлу swagger.json
    /// </summary>
    public required string RouteTemplate { get; set; }

    /// <summary>
    ///     Префикс для доступа к интерфейсу Swagger
    /// </summary>
    public required string RoutePrefix { get; set; }
}