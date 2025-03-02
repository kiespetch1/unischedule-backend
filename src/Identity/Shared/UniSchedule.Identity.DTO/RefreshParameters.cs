namespace UniSchedule.Identity.DTO;

/// <summary>
///     Параметры для обновления токена
/// </summary>
public class RefreshParameters
{
    /// <summary>
    ///     Токен с истекшим сроком действия
    /// </summary>
    public required string ExpiredToken { get; set; }

    /// <summary>
    ///     Токен обновления
    /// </summary>
    public required string RefreshToken { get; set; }
}