namespace UniSchedule.Bot.Entities.Settings;

public class VkApiSettings
{
    /// <summary>
    ///     Версия API
    /// </summary>
    public required string Version { get; set; }
    
    /// <summary>
    ///     Токен доступа
    /// </summary>
    public required string AccessToken { get; set; }

    /// <summary>
    ///     Код подтверждения
    /// </summary>
    public required string ConfirmationCode { get; set; }

    /// <summary>
    ///     Идентификатор группы бота
    /// </summary>
    public required string GroupId { get; set; }

    /// <summary>
    ///     Секретный ключ
    /// </summary>
    public required string Secret { get; set; }

    /// <summary>
    ///     Путь к файлу шаблона ответов от API
    /// </summary>
    public required string ResponseObjectTemplatesPath { get; set; }
}