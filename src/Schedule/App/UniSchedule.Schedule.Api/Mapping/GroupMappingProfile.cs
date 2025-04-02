using UniSchedule.Extensions.Mapping;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Models;

namespace UniSchedule.Schedule.Api.Mapping;

/// <summary>
///     Профиль маппера для <see cref="Group" />
/// </summary>
public class GroupMappingProfile : MappingProfileBase
{
    public GroupMappingProfile()
    {
        CreateMap<Group, GroupModel>();

        CreateMapForCollectionResult<Group, GroupModel>();
    }
}