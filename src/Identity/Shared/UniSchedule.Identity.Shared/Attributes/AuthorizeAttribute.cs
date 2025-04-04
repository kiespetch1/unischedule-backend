using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using UniSchedule.Extensions.Exceptions;

namespace UniSchedule.Identity.Shared.Attributes;

/// <summary>
///     Атрибут авторизации
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    /// <summary />
    public AuthorizeAttribute(RoleOption role)
    {
        Roles = new List<string>();
        Roles.Add(role.ToString());
    }

    /// <summary />
    public AuthorizeAttribute(params RoleOption[] roles)
    {
        Roles = new List<string>();
        Roles.AddRange(roles.Select(x => x.ToString()));
    }

    /// <summary>
    ///     Роли
    /// </summary>
    private List<string> Roles { get; }

    /// <inheritdoc />
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.User.Identity?.IsAuthenticated != true)
        {
            throw new NotAuthorizedException();
        }

        var claim = context.HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Role);
        if (claim == null)
        {
            throw new NoAccessRightsException();
        }

        var roles = JsonSerializer.Deserialize<List<string>>(claim.Value) ?? [];
        if (!roles.Any(role => Roles.Contains(role)))
        {
            throw new NoAccessRightsException();
        }
    }
}