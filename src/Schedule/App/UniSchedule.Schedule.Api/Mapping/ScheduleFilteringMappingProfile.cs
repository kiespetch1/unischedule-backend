using UniSchedule.Extensions.Mapping;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Api.Mapping;

public class ScheduleFilteringMappingProfile : MappingProfileBase
{
    /// <summary />
    public ScheduleFilteringMappingProfile()
    {
        CreateMap<ScheduleFilteringOption, ScheduleFilteringModel>();

        CreateMapForCollectionResult<ScheduleFilteringOption, ScheduleFilteringModel>();
    }
}