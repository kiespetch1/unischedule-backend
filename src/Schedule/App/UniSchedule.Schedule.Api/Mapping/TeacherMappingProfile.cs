using AutoMapper;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Models;

namespace UniSchedule.Schedule.Api.Mapping;

/// <summary>
///     Профиль маппинга для преподавателей
/// </summary>
public class TeacherMappingProfile : Profile
{
    /// <summary />
    public TeacherMappingProfile()
    {
        CreateMap<Teacher, TeacherModel>();
    }
}