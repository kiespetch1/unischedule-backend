namespace UniSchedule.Identity.Entities;

public class Token
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
}