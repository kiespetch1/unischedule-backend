using UniSchedule.Extensions.Mapping;
using UniSchedule.Schedule.Entities;
using UniSchedule.Schedule.Entities.Owned;
using UniSchedule.Shared.DTO.Models;

namespace UniSchedule.Schedule.Api.Mapping;

/// <summary>
///     Профиль маппера для <see cref="Announcement" />
/// </summary>
public class AnnouncementMappingProfile : MappingProfileBase
{
    public AnnouncementMappingProfile()
    {
        CreateMap<Announcement, AnnouncementModel>()
            .ForMember(x => x.CreatedBy, opt => opt.MapFrom(x => x.Creator))
            .ForMember(x => x.UpdatedBy, opt => opt.MapFrom(x => x.Updater));
        CreateMap<AnnouncementTargetInfo, AnnouncementTargetModel>();

        CreateMapForCollectionResult<AnnouncementTargetInfo, AnnouncementTargetModel>();
        CreateMapForCollectionResult<Announcement, AnnouncementModel>();
    }
}