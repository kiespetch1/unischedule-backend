namespace UniSchedule.Identity.DTO;

/// <summary>
///     Параметры для создания токена
/// </summary>
public class SignInParameters
{
    /// <summary>
    ///     Login/Email пользователя
    /// </summary>
    public required string Login { get; set; }

    /// <summary>
    ///     Пароль пользователя
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    ///     URL для перенаправления после успешной авторизации
    /// </summary>
    public string? ReturnUrl { get; set; }
}