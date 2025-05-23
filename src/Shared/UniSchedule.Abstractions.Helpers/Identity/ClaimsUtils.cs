using System.Security.Claims;
using System.Text.Json;
using ClaimTypes = UniSchedule.Identity.Shared.ClaimTypes;

namespace UniSchedule.Abstractions.Helpers.Identity;

/// <summary>
///     Статические методы для работы с Claims
/// </summary>
public static class ClaimsUtils
{
    /// <summary>
    ///     Создание <see cref="ClaimsIdentity" /> на основе <see cref="TokenIssueContext" />
    /// </summary>
    /// <param name="context">Контекст для выдачи токена доступа</param>
    /// <returns>
    ///     <see cref="ClaimsIdentity" />
    /// </returns>
    public static ClaimsIdentity CreateClaims(TokenIssueContext context)
    {
        var claims = new ClaimsIdentity();

        claims.AddClaim(new Claim(ClaimTypes.UserId, context.UserId.ToString()));
        claims.AddClaim(new Claim(ClaimTypes.Surname, context.Surname));
        claims.AddClaim(new Claim(ClaimTypes.Name, context.Name));
        claims.AddClaim(new Claim(ClaimTypes.Patronymic, context.Patronymic ?? string.Empty));
        claims.AddClaim(new Claim(ClaimTypes.Email, context.Email));
        claims.AddClaims((context.ManagedGroupIds ?? [])
            .Select(groupId => new Claim(ClaimTypes.ManagedGroupIds, groupId.ToString())));
        claims.AddClaim(new Claim(ClaimTypes.GroupId, context.GroupId.ToString() ?? Guid.Empty.ToString()));
        claims.AddClaim(new Claim(ClaimTypes.Role, JsonSerializer.Serialize(context.Role.Name)));

        return claims;
    }

    /// <summary>
    ///     Создание контекста пользователя на основе списка Claims
    /// </summary>
    /// <param name="claims">Список Claims</param>
    /// <returns>Контекст пользователя</returns>
    public static UserContext CreateContext(List<Claim> claims)
    {
        var userId = Guid.Parse(claims.Single(claim => claim.Type == ClaimTypes.UserId).Value);
        var surname = claims.Single(claim => claim.Type == ClaimTypes.Surname).Value;
        var name = claims.Single(claim => claim.Type == ClaimTypes.Name).Value;
        var patronymic = claims.SingleOrDefault(claim => claim.Type == ClaimTypes.Patronymic)?.Value ?? string.Empty;
        var email = claims.Single(claim => claim.Type.Contains(ClaimTypes.Email)).Value;
        var managedGroupIds = claims
            .Where(claim => claim.Type == ClaimTypes.ManagedGroupIds)
            .Select(claim => Guid.Parse(claim.Value))
            .ToList();
        var groupId = Guid.Parse(claims.Single(claim => claim.Type.Contains(ClaimTypes.GroupId)).Value);

        var rawRole = claims
                          .SingleOrDefault(c =>
                              c.Type == ClaimTypes.Role || c.Type == "role" || c.Type ==
                              "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value
                      ?? throw new InvalidOperationException("Клейм роли не найден");
        var role = JsonSerializer.Deserialize<string>(rawRole)
                   ?? throw new InvalidOperationException("Клейм роли имеет неверный формат");
        
        return new UserContext(
            userId,
            surname,
            name,
            patronymic,
            email,
            managedGroupIds,
            groupId,
            role);
    }
}