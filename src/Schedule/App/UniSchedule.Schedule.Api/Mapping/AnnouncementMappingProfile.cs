using UniSchedule.Extensions.Mapping;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Models;

namespace UniSchedule.Schedule.Api.Mapping;

/// <summary>
///     Профиль маппера для <see cref="Announcement" />
/// </summary>
public class AnnouncementMappingProfile : MappingProfileBase
{
    public AnnouncementMappingProfile()
    {
        CreateMap<Announcement, AnnouncementModel>();

        CreateMapForCollectionResult<Announcement, AnnouncementModel>();
    }
}