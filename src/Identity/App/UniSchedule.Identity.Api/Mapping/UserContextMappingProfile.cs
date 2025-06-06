﻿using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Extensions.Mapping;
using UniSchedule.Identity.DTO.Models;

namespace UniSchedule.Identity.Api.Mapping;

/// <summary>
///     Профиль маппера для <see cref="UserContext" />
/// </summary>
public class UserContextMappingProfile : MappingProfileBase
{
    /// <summary />
    public UserContextMappingProfile()
    {
        CreateMap<UserContext, UserContextModel>();

        CreateMapForCollectionResult<UserContext, UserContextModel>();
    }
}