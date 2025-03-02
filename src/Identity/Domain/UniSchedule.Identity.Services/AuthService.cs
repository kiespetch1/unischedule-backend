using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UniSchedule.Extensions.Collections;
using UniSchedule.Extensions.Exceptions;
using UniSchedule.Identity.Database;
using UniSchedule.Identity.DTO;
using UniSchedule.Identity.Entities;
using UniSchedule.Identity.Services.Abstractions;

namespace UniSchedule.Identity.Services;

public class AuthService(
    DatabaseContext context,
    ITokenContextProvider tokenContextProvider,
    ITokenProvider tokenProvider) : IAuthService
{
    /// <inheritdoc />
    public async Task<TokenModel> SignInAsync(
        SignInParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var user = await GetUserAsync(u => u.Email == parameters.Login, cancellationToken);

        var tokenContext = tokenContextProvider.CreateContext(user);
        var token = tokenProvider.IssueToken(tokenContext, parameters.ReturnUrl);

        user.RefreshToken = token.RefreshToken;
        await context.SaveChangesAsync(cancellationToken);

        return token;
    }

    /// <inheritdoc />
    public async Task<TokenModel> RefreshAsync(
        RefreshParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var userContext = tokenProvider.ParseToken(parameters.ExpiredToken);
        var user = await GetUserAsync(u => u.Id == userContext.Id, cancellationToken);

        if (user.RefreshToken != parameters.RefreshToken)
        {
            throw new NoAccessRightsException();
        }

        var tokenContext = tokenContextProvider.CreateContext(user);
        var token = tokenProvider.IssueToken(tokenContext);

        user.RefreshToken = token.RefreshToken;
        await context.SaveChangesAsync(cancellationToken);

        return token;
    }

    /// <summary>
    ///     Получение пользователя по предикату условия
    /// </summary>
    /// <param name="predicate">Предикат условия</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Пользователь</returns>
    private async Task<User> GetUserAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Include(x => x.Role)
            .SingleOrNotFoundAsync(predicate, cancellationToken);

        return user;
    }
}