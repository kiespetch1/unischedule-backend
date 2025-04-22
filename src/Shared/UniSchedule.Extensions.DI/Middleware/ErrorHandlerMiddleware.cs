using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UniSchedule.Extensions.Data;
using UniSchedule.Extensions.Exceptions.Base;
using UniSchedule.Extensions.Utils;

namespace UniSchedule.Extensions.DI.Middleware;

/// <summary>
///     Обработчик ошибок
/// </summary>
/// <param name="next">Следующий Middleware</param>
/// <param name="logger">Логгер</param>
public class ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
{
    /// <summary />
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json; charset=utf-8";

            var result = new Result<object>(null);

            switch (error)
            {
                case RequestException e:
                    response.StatusCode = (int)e.StatusCode;
                    result.Error = new Error(e);
                    break;
                case AntiforgeryValidationException e:
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    result.Error = new Error(e);
                    break;
                default:
                    var message = $"{error.GetType()}: {error.Message}\n{error.StackTrace}";
                    logger.LogError(message);
                    message = EnvironmentUtils.IsProduction ? "Внутренняя ошибка сервера" : message;
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    result.Error = new Error { Message = message };
                    break;
            }

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };
            await response.WriteAsync(JsonSerializer.Serialize(result, serializerOptions));
        }
    }
}