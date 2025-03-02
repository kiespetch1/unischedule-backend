using UniSchedule.Identity.DTO;

namespace UniSchedule.Identity.Services.Abstractions;

/// <summary>
///     Сервис для работы с токенами авторизации
/// </summary>
public interface IAuthService
{
    /// <summary>
    ///     Авторизация
    /// </summary>
    /// <param name="parameters">Параметры создания токена авторизации</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Токен авторизации</returns>
    Task<TokenModel> SignInAsync(SignInParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Обновление токена авторизации
    /// </summary>
    /// <param name="parameters">Параметры обновления токена авторизации</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Токен авторизации</returns>
    Task<TokenModel> RefreshAsync(RefreshParameters parameters, CancellationToken cancellationToken = default);
}