using UniSchedule.Extensions.Mapping;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Models;

namespace UniSchedule.Schedule.Api.Mapping;

/// <summary>
///     Профиль маппера для <see cref="Class" />
/// </summary>
public class ClassMappingProfile : MappingProfileBase
{
    public ClassMappingProfile()
    {
        CreateMap<Class, ClassModel>();

        CreateMapForCollectionResult<Class, ClassModel>();
    }
}