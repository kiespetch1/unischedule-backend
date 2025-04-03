using UniSchedule.Entities;
using UniSchedule.Extensions.Mapping;
using UniSchedule.Shared.DTO.Models;

namespace UniSchedule.Schedule.Api.Mapping;

/// <summary>
///     Профиль маппера для <see cref="User" />
/// </summary>
public class UserMappingProfile : MappingProfileBase
{
    public UserMappingProfile()
    {
        CreateMap<User, UserModel>();

        CreateMapForCollectionResult<User, UserModel>();
    }
}