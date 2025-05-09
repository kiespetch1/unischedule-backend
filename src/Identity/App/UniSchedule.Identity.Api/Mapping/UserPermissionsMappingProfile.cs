using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Extensions.Mapping;
using UniSchedule.Identity.DTO.Models;

namespace UniSchedule.Identity.Api.Mapping;

/// <summary>
///     Профиль маппера для <see cref="UserContext" />
/// </summary>
public class UserPermissionsMappingProfile : MappingProfileBase
{
    /// <summary />
    public UserPermissionsMappingProfile()
    {
        CreateMap<UserPermissions, UserPermissionsModel>();

        CreateMapForCollectionResult<UserPermissions, UserPermissionsModel>();
    }
}