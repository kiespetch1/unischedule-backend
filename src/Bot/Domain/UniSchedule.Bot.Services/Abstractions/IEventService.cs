using UniSchedule.Bot.Entities.Vk;
using UniSchedule.Bot.Shared;
using UniSchedule.Entities.DTO;

namespace UniSchedule.Bot.Services.Abstractions;

/// <summary>
///     Сервис обработки событий из мессенджеров
/// </summary>
public interface IEventService
{
    /// <summary>
    ///     Обработка события
    /// </summary>
    /// <param name="event">Объект события</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<string> HandleEventAsync(VkEventParameters @event, CancellationToken cancellationToken = default);

    /// <summary>
    ///    Авторизация беседы и пользователя в боте 
    /// </summary>
    /// <param name="parameters">Параметры привязки бота к беседе в мессенджере</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public Task LinkMessengerAsync(MessengerLinkParameters parameters, CancellationToken cancellationToken = default);
    
    /// <summary>
    ///     Получение списка бесед
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список бесед</returns>
    public Task<List<KeyValueItem<long>>> GetConversationsListAsync(CancellationToken cancellationToken = default);
}