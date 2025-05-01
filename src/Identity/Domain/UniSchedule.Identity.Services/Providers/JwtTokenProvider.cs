using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Extensions.DI.Settings.Auth;
using UniSchedule.Identity.Entities;
using UniSchedule.Identity.Services.Abstractions;

namespace UniSchedule.Identity.Services.Providers;

public class JwtTokenProvider(JwtTokenSettings settings) : ITokenProvider
{
    /// <inheritdoc />
    public Token IssueToken(TokenIssueContext context)
    {
        var claims = ClaimsUtils.CreateClaims(context);
        var accessToken = IssueToken(claims);
        var refreshToken = HashData(context.UserId.ToString());

        return new Token
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiredAt = DateTime.UtcNow.Add(settings.Lifetime),
        };
    }

    /// <inheritdoc />
    public UserContext ParseToken(string accessToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(accessToken);
        var userContext = ClaimsUtils.CreateContext(token.Claims.ToList());

        return userContext;
    }

    /// <summary>
    ///     Формирование строкового значения jwt-токена доступа
    /// </summary>
    /// <param name="claims">
    ///     <see cref="ClaimsIdentity" />
    /// </param>
    /// <returns>Строковое значение jwt-токена доступа</returns>
    private string IssueToken(ClaimsIdentity claims)
    {
        var handler = new JwtSecurityTokenHandler();
        var expiredAt = DateTime.UtcNow.Add(settings.Lifetime);
        var issuedAt = DateTime.UtcNow;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecurityKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var jwtToken = handler.CreateJwtSecurityToken(
            subject: claims,
            signingCredentials: signingCredentials,
            audience: settings.Audience,
            issuer: settings.Issuer,
            issuedAt: issuedAt,
            expires: expiredAt);
        var accessToken = handler.WriteToken(jwtToken);

        return accessToken;
    }

    /// <summary>
    ///     Хэширование строки
    /// </summary>
    /// <param name="value">Строка, которую необходимо хэшировать</param>
    /// <returns>Хэш строки</returns>
    private static string HashData(string value)
    {
        var issuedAt = DateTime.UtcNow;
        var rawValue = Encoding.UTF8.GetBytes(value + issuedAt.ToString("u"));
        var hash = SHA256.HashData(rawValue);

        var stringBuilder = new StringBuilder();
        foreach (var num in hash)
        {
            stringBuilder.Append(num.ToString("x2").ToLower());
        }

        return stringBuilder.ToString();
    }
}