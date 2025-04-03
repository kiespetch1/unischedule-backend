using System.Text.Json;
using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Extensions.Mapping;
using UniSchedule.Identity.DTO.Models;
using UniSchedule.Identity.Entities;

namespace UniSchedule.Identity.Api.Mapping;

/// <summary>
///     Профиль маппера для <see cref="UserContext" />
/// </summary>
public class UserContextMappingProfile : MappingProfileBase
{
    /// <summary />
    public UserContextMappingProfile()
    {
        CreateMap<UserContext, UserContextModel>()
            .ForMember(x => x.Role, opt => opt.MapFrom(x => JsonSerializer.Deserialize<Role>(x.Role, (JsonSerializerOptions)null!)));

        CreateMapForCollectionResult<UserContext, UserContextModel>();
    }
}