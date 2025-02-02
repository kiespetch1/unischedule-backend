namespace UniSchedule.Extensions.DI.Settings.ApiDocumentation;

/// <summary>
///     Настройки Scalar
/// </summary>
public class ScalarSettings
{
    /// <summary>
    ///     Префикс для доступа к интерфейсу Scalar
    /// </summary>
    public required string RoutePrefix { get; set; }
    
    /// <summary>
    ///     Наименование страницы Scalar
    /// </summary>
    public required string DocumentTitle { get; set; }
}