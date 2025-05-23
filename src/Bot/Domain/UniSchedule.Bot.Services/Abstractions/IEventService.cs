﻿using UniSchedule.Bot.Entities.Vk;
using UniSchedule.Bot.Shared;

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
    Task<string> HandleEventAsync(VkEvent @event, CancellationToken cancellationToken = default);

    /// <summary>
    ///    Авторизация беседы и пользователя в боте 
    /// </summary>
    /// <param name="parameters">Параметры привязки бота к беседе в мессенджере</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public Task LinkMessengerAsync(MessengerLinkParameters parameters, CancellationToken cancellationToken);
}