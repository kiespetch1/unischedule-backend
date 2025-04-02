using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UniSchedule.Abstractions.Commands;
using UniSchedule.Abstractions.Queries.Base;
using UniSchedule.Extensions.Attributes;
using UniSchedule.Extensions.Data;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Models;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Api.Controllers;

/// <summary>
///     Контроллер для работы с объявлениями
/// </summary>
[ApiController]
[Route("/api/v1/[controller]")]
public class AnnouncementsController(
    ICreateCommand<Announcement, AnnouncementCreateParameters, Guid> create,
    IDeleteCommand<Announcement, Guid> delete,
    IUpdateCommand<Announcement, AnnouncementUpdateParameters, Guid> update,
    IMultipleQuery<Announcement, AnnouncementQueryParameters> get,
    ISingleQuery<Announcement, Guid> getById,
    IMapper mapper)
{
    /// <summary>
    ///     Создание объявления
    /// </summary>
    /// <param name="parameters">Параметры создания объявления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор объявления</returns>
    /// <response code="200">Успешное создание объявления</response>
    /// <response code="400">Некорректные данные</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPost]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.InternalServerError)]
    public async Task<Result<Guid>> CreateAsync(
        [FromBody] AnnouncementCreateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var id = await create.ExecuteAsync(parameters, cancellationToken);

        return new Result<Guid>(id);
    }

    /// <summary>
    ///     Получение объявления по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объявления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объявление</returns>
    /// <response code="200">Успешное получение объявления</response>
    /// <response code="404">Объявление не найдено</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpGet("{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    public async Task<Result<AnnouncementModel>> GetByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var announcement = await getById.ExecuteAsync(id, cancellationToken);
        var result = mapper.Map<AnnouncementModel>(announcement);

        return new Result<AnnouncementModel>(result);
    }

    /// <summary>
    ///     Получение списка объявлений
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список объявлений</returns>
    /// <response code="200">Успешное получение объявлений</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpGet]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.InternalServerError)]
    public async Task<CollectionResult<AnnouncementModel>> GetAsync(
        [FromQuery] AnnouncementQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var announcements = await get.ExecuteAsync(parameters, cancellationToken);
        var data = mapper.Map<CollectionResult<AnnouncementModel>>(announcements);

        return data;
    }

    /// <summary>
    ///     Обновление объявления
    /// </summary>
    /// <param name="id">Идентификатор объявления</param>
    /// <param name="parameters">Параметры обновления объявления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат операции</returns>
    /// <response code="200">Успешное обновление объявления</response>
    /// <response code="400">Некорректные данные</response>
    /// <response code="404">Объявление не найдено</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPut("{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    public async Task UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] AnnouncementUpdateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        await update.ExecuteAsync(id, parameters, cancellationToken);
    }

    /// <summary>
    ///     Удаление объявления
    /// </summary>
    /// <param name="id">Идентификатор объявления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат операции</returns>
    /// <response code="200">Успешное удаление объявления</response>
    /// <response code="404">Объявление не найдено</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpDelete("{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    public async Task DeleteAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        await delete.ExecuteAsync(id, cancellationToken);
    }
}