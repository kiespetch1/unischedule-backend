using Microsoft.AspNetCore.Http;

namespace UniSchedule.Abstractions.Helpers.Identity;

/// <inheritdoc />
public class UserContextProvider(IHttpContextAccessor httpContextAccessor) : IUserContextProvider
{
    /// <inheritdoc />
    public bool IsAuthenticated()
    {
        var context = httpContextAccessor.HttpContext;

        return context?.User.Identity?.IsAuthenticated ?? false;
    }

    /// <inheritdoc />
    public UserContext GetContext()
    {
        var context = httpContextAccessor.HttpContext;
        var isAuthenticated = context?.User.Identity?.IsAuthenticated ?? false;
        var claims = context?.User.Claims.ToList();

        return !isAuthenticated || claims == null
            ? new UserContext()
            : ClaimsUtils.CreateContext(claims);
    }
}