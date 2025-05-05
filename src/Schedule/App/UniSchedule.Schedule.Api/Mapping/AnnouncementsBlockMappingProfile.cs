using UniSchedule.Extensions.Mapping;
using UniSchedule.Schedule.Entities.Owned;
using UniSchedule.Shared.DTO.Models;

namespace UniSchedule.Schedule.Api.Mapping;

/// <summary>
///     Профиль маппера для <see cref="AnnouncementsBlock" />
/// </summary>
public class AnnouncementsBlockMappingProfile : MappingProfileBase
{
    public AnnouncementsBlockMappingProfile()
    {
        CreateMap<AnnouncementsBlock, AnnouncementsBlockModel>();

        CreateMapForCollectionResult<AnnouncementsBlock, AnnouncementsBlockModel>();
    }
}