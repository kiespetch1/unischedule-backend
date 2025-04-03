namespace UniSchedule.Identity.DTO.Models;

/// <summary>
///     Модель токена
/// </summary>
public class TokenModel
{
    /// <summary>
    ///     Токен
    /// </summary>
    public required string AccessToken { get; set; }

    /// <summary>
    ///     Токен обновления
    /// </summary>
    public string RefreshToken { get; set; }

    /// <summary>
    ///     Время жизни токена
    /// </summary>
    public DateTime ExpiredAt { get; set; }

    /// <summary>
    ///     Схема токена
    /// </summary>
    public string Scheme { get; set; } = "Bearer";

    /// <summary>
    ///     URL для перенаправления после успешной авторизации
    /// </summary>
    public string? ReturnUrl { get; set; }
}