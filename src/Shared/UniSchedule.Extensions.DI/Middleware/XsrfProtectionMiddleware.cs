using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace UniSchedule.Extensions.DI.Middleware;

/// <summary>
///     Выдаёт клиенту на каждый запрос новый request‑токен
/// </summary>
public class XsrfProtectionMiddleware(RequestDelegate next, IAntiforgery antiforgery)
{
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Append("X-Xss-Protection", "1");
        context.Response.Headers.Append("X-Frame-Options", "DENY");

        var tokens = antiforgery.GetAndStoreTokens(context);

        context.Response.Headers.Append("XSRF-TOKEN", tokens.RequestToken!);

        await next(context);
    }
}