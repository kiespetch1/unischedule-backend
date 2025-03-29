using UniSchedule.Extensions.Mapping;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Models;

namespace UniSchedule.Schedule.Api.Mapping;

/// <summary>
///     Профиль маппера для <see cref="Day" />
/// </summary>
public class DayMappingProfile : MappingProfileBase
{
    public DayMappingProfile()
    {
        CreateMap<Day, DayModel>();

        CreateMapForCollectionResult<Day, DayModel>();
    }
}