using UniSchedule.Extensions.Mapping;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Models;

namespace UniSchedule.Schedule.Api.Mapping;

/// <summary>
///     Профиль маппера для <see cref="Location" />
/// </summary>
public class LocationMappingProfile : MappingProfileBase
{
    public LocationMappingProfile()
    {
        CreateMap<Location, LocationModel>();

        CreateMapForCollectionResult<Location, LocationModel>();
    }
}