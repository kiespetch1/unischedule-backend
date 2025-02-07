namespace UniSchedule.Extensions.Utils;

/// <summary>
///     Утилиты для работы с <see cref="Environment" />
/// </summary>
public static class EnvironmentUtils
{
    /// <summary>
    ///     Приложение запущено в Production-среде
    /// </summary>
    public static bool IsProduction => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";

    /// <summary>
    ///     Приложение запущено в Development-среде
    /// </summary>
    public static bool IsDevelopment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

    /// <summary>
    ///     Приложение запущено в Staging-среде
    /// </summary>
    public static bool IsStaging => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Staging";

    /// <summary>
    ///     Дефолтный идентификатор сервиса
    /// </summary>
    public static Guid DefaultId =>
        Guid.Parse(Environment.GetEnvironmentVariable("SERVICE_ID") ?? Guid.Empty.ToString());

    /// <summary>
    ///     Дефолтный идентификатор сервиса
    /// </summary>
    public static string ServiceName => Environment.GetEnvironmentVariable("SERVICE_NAME") ?? string.Empty;
}