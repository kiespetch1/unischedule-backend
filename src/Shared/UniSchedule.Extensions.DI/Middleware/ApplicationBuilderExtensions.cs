using Microsoft.AspNetCore.Builder;

namespace UniSchedule.Extensions.DI.Middleware;

/// <summary>
///     Методы расширения для ApplicationBuilder
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    ///     Добавление обработчика исключений
    /// </summary>
    /// <param name="app">Приложение</param>
    public static void UseErrorHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();
    }
}