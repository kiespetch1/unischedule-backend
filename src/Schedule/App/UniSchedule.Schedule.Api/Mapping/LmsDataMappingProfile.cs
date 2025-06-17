using UniSchedule.Extensions.Mapping;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Models;

namespace UniSchedule.Schedule.Api.Mapping;

/// <summary>
///     Профиль маппера для <see cref="LmsData" />
/// </summary>
public class LmsDataMappingProfile : MappingProfileBase
{
    public LmsDataMappingProfile()
    {
        CreateMap<LmsData, LmsDataModel>();

        CreateMapForCollectionResult<LmsData, LmsDataModel>();
    }
}