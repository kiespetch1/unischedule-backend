namespace UniSchedule.Extensions.DI.Settings.Auth;

/// <summary>
///     Настройки авторизации
/// </summary>
public class JwtTokenSettings
{
    /// <summary>
    ///     Издатель токена
    /// </summary>
    public required string Issuer { get; set; }

    /// <summary>
    ///     Название группы токена
    /// </summary>
    public required string Audience { get; set; }

    /// <summary>
    ///     Время жизни токена
    /// </summary>
    public TimeSpan Lifetime { get; set; }

    /// <summary>
    ///     Ключ шифрования токена
    /// </summary>
    public required string SecurityKey { get; set; }
}