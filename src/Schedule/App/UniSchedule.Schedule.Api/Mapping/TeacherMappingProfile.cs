using UniSchedule.Extensions.Mapping;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Models;

namespace UniSchedule.Schedule.Api.Mapping;

/// <summary>
///     Профиль маппинга для преподавателей
/// </summary>
public class TeacherMappingProfile : MappingProfileBase
{
    /// <summary />
    public TeacherMappingProfile()
    {
        CreateMap<Teacher, TeacherModel>();

        CreateMapForCollectionResult<Teacher, TeacherModel>();
    }
}