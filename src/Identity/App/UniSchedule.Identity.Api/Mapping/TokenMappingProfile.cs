using UniSchedule.Extensions.Mapping;
using UniSchedule.Identity.DTO.Models;
using UniSchedule.Identity.Entities;

namespace UniSchedule.Identity.Api.Mapping;

public class TokenMappingProfile : MappingProfileBase
{
    public TokenMappingProfile()
    {
        CreateMap<Token, TokenModel>();

        CreateMapForCollectionResult<Token, TokenModel>();
    }
}