using System.Net;
using Microsoft.AspNetCore.Mvc;
using UniSchedule.Extensions.Attributes;

namespace UniSchedule.Events.Api.Controllers;

/// <summary>
///     API для работы с авторизацией
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class AccountController : ControllerBase
{
    /// <summary>
    ///     Обновление antiforgery токена
    /// </summary>
    /// <response code="200">Успешное обновление токена</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPost("antiforgery")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.InternalServerError)]
    public void RefreshAntiforgery()
    {
    }
}