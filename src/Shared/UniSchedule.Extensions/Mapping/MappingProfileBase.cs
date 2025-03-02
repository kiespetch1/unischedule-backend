using AutoMapper;
using UniSchedule.Extensions.Data;

namespace UniSchedule.Extensions.Mapping;

/// <summary>
///     Базовый профиль для Automapper
/// </summary>
public abstract class MappingProfileBase : Profile
{
    /// <summary>
    ///     Создание маппинга для <see cref="CollectionResult{T}" />
    /// </summary>
    /// <typeparam name="TSource">Исходный тип</typeparam>
    /// <typeparam name="TDestination">Целевой тип</typeparam>
    protected void CreateMapForCollectionResult<TSource, TDestination>()
    {
        CreateMap<CollectionResult<TSource>, CollectionResult<TDestination>>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));
    }
}