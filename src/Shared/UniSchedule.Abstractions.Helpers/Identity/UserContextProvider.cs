using Microsoft.AspNetCore.Http;
using UniSchedule.Identity.Shared;

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

        if (!isAuthenticated || claims == null)
        {
            return new UserContext();
        }
        else
        {
            var result = ClaimsUtils.CreateContext(claims);

            return result;
        }
    }

    /// <inheritdoc />
    public UserPermissions GetPermissions()
    {
        var role = GetContext().Role;

        return role switch
        {
            nameof(RoleOption.Staff) or nameof(RoleOption.GroupLeader) =>
                new UserPermissions
                {
                    CanGetCurrentUser = true,
                    CanCreateAnnouncement = true,
                    CanUpdateAnnouncement = true,
                    CanDeleteAnnouncement = true,
                    CanCreateClass = true,
                    CanUpdateClass = true,
                    CanDeleteClass = true,
                    CanCancelClass = true,
                    CanRestoreClass = true,
                    CanCopyClass = true,
                    CanCreateLocation = true,
                    CanUpdateLocation = true,
                    CanDeleteLocation = true,
                    CanCreateTeacher = true,
                    CanUpdateTeacher = true,
                    CanDeleteTeacher = true
                },
            nameof(RoleOption.Admin) =>
                new UserPermissions
                {
                    CanRegisterUser = true,
                    CanUpdateUser = true,
                    CanGetCurrentUser = true,
                    CanCreateAnnouncement = true,
                    CanUpdateAnnouncement = true,
                    CanDeleteAnnouncement = true,
                    CanCreateClass = true,
                    CanUpdateClass = true,
                    CanDeleteClass = true,
                    CanCancelClass = true,
                    CanRestoreClass = true,
                    CanCopyClass = true,
                    CanCreateGroup = true,
                    CanUpdateGroup = true,
                    CanDeleteGroup = true,
                    CanUpdateGrades = true,
                    CanCreateLocation = true,
                    CanUpdateLocation = true,
                    CanDeleteLocation = true,
                    CanCreateTeacher = true,
                    CanUpdateTeacher = true,
                    CanDeleteTeacher = true,
                    CanCreateWeek = true,
                    CanDeleteWeek = true
                },
            _ => new UserPermissions()
        };
    }
}