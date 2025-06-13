using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Identity.Entities;
using UniSchedule.Identity.Services.Abstractions;

namespace UniSchedule.Identity.Services.Providers;

/// <inheritdoc />
public class TokenContextProvider : ITokenContextProvider
{
    /// <inheritdoc />
    public TokenIssueContext CreateContext(User user)
    {
        var tokenContext = new TokenIssueContext
        {
            UserId = user.Id,
            Surname = user.Surname,
            Name = user.Name,
            Patronymic = user.Patronymic,
            Email = user.Email,
            GroupId = user.GroupId,
            Role = user.Role,
            ManagedGroupIds = user.ManagedGroupIds
        };

        return tokenContext;
    }
}