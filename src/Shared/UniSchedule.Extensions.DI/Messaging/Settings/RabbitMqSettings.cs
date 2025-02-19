namespace UniSchedule.Extensions.DI.Messaging.Settings;

/// <summary>
///     Настройки для RabbitMQ
/// </summary>
public class RabbitMqSettings
{
    /// <summary>
    ///     Хост
    /// </summary>
    public required string Host { get; set; }

    /// <summary>
    ///     Имя пользователя
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    ///     Пароль
    /// </summary>
    public required string Password { get; set; }
}