using System.Net;
using Microsoft.AspNetCore.Mvc;
using UniSchedule.Bot.Entities.Vk;
using UniSchedule.Bot.Services.Abstractions;
using UniSchedule.Bot.Shared;
using UniSchedule.Extensions.Attributes;
using UniSchedule.Identity.Shared;
using UniSchedule.Identity.Shared.Attributes;

namespace UniSchedule.Bot.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsController(IEventService eventService) : ControllerBase
{
    /// <summary>
    ///     Обработка входящих событий
    /// </summary>
    /// <param name="vkEvent">Объект события</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>"ok" после обработки запроса</returns>
    /// <response code="200">Успешная обработка события</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPost("handle")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> HandleEventAsync(
        [FromBody] VkEvent vkEvent,
        CancellationToken cancellationToken = default)
    {
        var result = await eventService.HandleEventAsync(vkEvent, cancellationToken);

        return Ok(result);
    }
    
    /// <summary>
    ///     Авторизация беседы и пользователя в боте
    /// </summary>
    /// <param name="parameters">Параметры привязки бота к беседе в мессенджере</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <response code="200">Успешная обработка события</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPost("auth")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin, RoleOption.GroupLeader, RoleOption.Staff)]
    public async Task LinkMessengerAsync(
        [FromBody] MessengerLinkParameters parameters,
        CancellationToken cancellationToken = default) 
    {
        await eventService.LinkMessengerAsync(parameters, cancellationToken);
    }
}