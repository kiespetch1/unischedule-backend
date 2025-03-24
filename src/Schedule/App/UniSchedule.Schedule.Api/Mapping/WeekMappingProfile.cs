using UniSchedule.Extensions.Mapping;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Models;

namespace UniSchedule.Schedule.Api.Mapping;

/// <summary>
///     Профиль маппера для <see cref="Week" />
/// </summary>
public class WeekMappingProfile : MappingProfileBase
{
    public WeekMappingProfile()
    {
        CreateMap<Week, WeekModel>();

        CreateMapForCollectionResult<Week, WeekModel>();
    }
}