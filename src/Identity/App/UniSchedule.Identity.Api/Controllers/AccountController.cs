using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Extensions.Attributes;
using UniSchedule.Extensions.Data;
using UniSchedule.Extensions.Exceptions;
using UniSchedule.Identity.DTO.Models;
using UniSchedule.Identity.DTO.Parameters;
using UniSchedule.Identity.Services.Abstractions;

namespace UniSchedule.Identity.Api.Controllers;

/// <summary>
///     API для работы с авторизацией
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class AccountController(
    IAuthService authenticationService,
    IUserContextProvider userContextProvider,
    IMapper mapper) : ControllerBase
{
    /// <summary>
    ///     Авторизация
    /// </summary>
    /// <param name="parameters">Параметры авторизации</param>
    /// <returns>Модель токена</returns>
    /// <response code="400">Некорректные данные</response>
    /// <response code="404">Пользователь не найден</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPost("sign_in")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    public async Task SignInAsync(SignInParameters parameters)
    {
        var token = await authenticationService.SignInAsync(parameters);

        HttpContext.Response.Cookies.Append("x-token", token.AccessToken,
            new CookieOptions { HttpOnly = true, MaxAge = TimeSpan.FromMinutes(30), Domain = ".streaminginfo.ru" });
        HttpContext.Response.Cookies.Append("z-token", token.RefreshToken,
            new CookieOptions { HttpOnly = true, MaxAge = TimeSpan.FromDays(30), Domain = ".streaminginfo.ru" });
    }

    /// <summary>
    ///     Обновление токена
    /// </summary>
    /// <returns>Модель токена</returns>
    /// <response code="200">Успешное обновление токена</response>
    /// <response code="400">Некорректные данные</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">Доступ запрещен</response>
    /// <response code="404">Пользователь не найден</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPost("refresh")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.Forbidden,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    public async Task RefreshAsync()
    {
        var refreshToken = HttpContext.Request.Cookies["z-token"];
        var expiredToken = HttpContext.Request.Cookies["x-token"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new NotAuthorizedException("Refresh token not found");
        }

        if (string.IsNullOrEmpty(expiredToken))
        {
            throw new NotAuthorizedException("Access token not found");
        }

        var parameters = new RefreshParameters { ExpiredToken = expiredToken, RefreshToken = refreshToken };

        var token = await authenticationService.RefreshAsync(parameters);

        HttpContext.Response.Cookies.Append("x-token", token.AccessToken,
            new CookieOptions { HttpOnly = true, MaxAge = TimeSpan.FromMinutes(30), Domain = ".streaminginfo.ru" });
        HttpContext.Response.Cookies.Append("z-token", token.RefreshToken,
            new CookieOptions { HttpOnly = true, MaxAge = TimeSpan.FromDays(30), Domain = ".streaminginfo.ru" });
    }

    /// <summary>
    ///     Получение информации о текущем пользователе
    /// </summary>
    /// <returns>Сокращенная модель "Пользователь"</returns>
    /// <response code="200">Успешное получение информации</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpGet("me")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.InternalServerError)]
    [Authorize]
    public Task<Result<UserContextModel>> GetCurrentAsync()
    {
        var currentUserContext = userContextProvider.GetContext();
        var user = mapper.Map<UserContextModel>(currentUserContext);

        return Task.FromResult(new Result<UserContextModel>(user));
    }
}