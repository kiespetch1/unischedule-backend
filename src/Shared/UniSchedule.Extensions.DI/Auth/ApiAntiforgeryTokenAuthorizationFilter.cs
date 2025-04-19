using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using UniSchedule.Identity.Shared.Attributes;

namespace UniSchedule.Extensions.DI.Auth;

public class ApiAntiforgeryTokenAuthorizationFilter(IAntiforgery antiforgery)
    : IAsyncAuthorizationFilter, IAntiforgeryPolicy
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var endpoint = context.HttpContext.GetEndpoint();
        if (endpoint == null)
        {
            await Task.CompletedTask;
            return;
        }

        var hasAuthorize = endpoint.Metadata.OfType<AuthorizeAttribute>().Any();
        if (!hasAuthorize)
        {
            return;
        }

        var method = context.HttpContext.Request.Method;
        if (HttpMethods.IsGet(method) ||
            HttpMethods.IsHead(method) ||
            HttpMethods.IsOptions(method) ||
            HttpMethods.IsTrace(method))
        {
            return;
        }

        if (!context.IsEffectivePolicy<IAntiforgeryPolicy>(this))
        {
            return;
        }

        try
        {
            await antiforgery.ValidateRequestAsync(context.HttpContext);
        }
        catch (AntiforgeryValidationException exception)
        {
            context.Result = new AntiforgeryValidationFailedResult();
        }
    }
}