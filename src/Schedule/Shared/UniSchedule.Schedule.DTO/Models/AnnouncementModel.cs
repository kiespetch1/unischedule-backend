namespace UniSchedule.Shared.DTO.Models;

/// <summary>
///     Модель "Объявление"
/// </summary>
public class AnnouncementModel
{
    /// <summary>
    ///     Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Текст объявления
    /// </summary>
    public required string Message { get; set; }

    /// <summary>
    ///     Является ли анонимным
    /// </summary>
    public bool IsAnonymous { get; set; }

    /// <summary>
    ///     Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///    Данные о создателе
    /// </summary>
    public UserModel? CreatedBy { get; set; }

    /// <summary>
    ///     Дата обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    ///     Данные об обновителе
    /// </summary>
    public UserModel? UpdatedBy { get; set; }
}