namespace UniSchedule.Extensions.DI.Settings.Auth;

/// <summary>
///     Класс для представления данных пользователя-администратора
/// </summary>
public class AdminCredentials
{
    /// <summary>
    ///     Эл. почта
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    ///     Пароль
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    ///     Фамилия
    /// </summary>
    public string Surname { get; set; }

    /// <summary>
    ///     Имя
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Отчество
    /// </summary>
    public string Patronymic { get; set; }
}