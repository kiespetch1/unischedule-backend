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

[ApiController]
[Route("/api/v1/[controller]")]
public class WeeksController(
    ICreateCommand<Week, WeekCreateParameters, Guid> create,
    IDeleteCommand<Week, Guid> delete,
    ISingleQuery<Week, Guid> getById,
    IMultipleQuery<Week, WeekQueryParameters> get,
    IMapper mapper)
{
    /// <summary>
    ///     Создание недели
    /// </summary>
    /// <param name="parameters">Параметры создания недели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор недели</returns>
    /// <response code="200">Успешное создание недели</response>
    /// <response code="400">Некорректные данные</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPost]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.InternalServerError)]
    public async Task<Result<Guid>> CreateAsync(
        [FromBody] WeekCreateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var id = await create.ExecuteAsync(parameters, cancellationToken);

        return new Result<Guid>(id);
    }

    /// <summary>
    ///     Удаление недели
    /// </summary>
    /// <param name="id">Идентификатор недели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат операции</returns>
    /// <response code="200">Успешное удаление недели</response>
    /// <response code="404">Неделя не найдена</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpDelete("{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        await delete.ExecuteAsync(id, cancellationToken);
    }

    /// <summary>
    ///     Получение недели по идентификатору
    /// </summary>
    /// <param name="id">Идентификаторы недели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Неделя</returns>
    /// <response code="200">Успешное получение недели</response>
    /// <response code="404">Неделя не найдена</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpGet("{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    public async Task<WeekModel> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var week = await getById.ExecuteAsync(id, cancellationToken);
        var data = mapper.Map<WeekModel>(week);

        return data;
    }

    /// <summary>
    ///     Получение списка недель
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список недель</returns>
    /// <response code="200">Успешное получение недель</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpGet]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.InternalServerError)]
    public async Task<CollectionResult<WeekModel>> GetAsync(
        [FromQuery] WeekQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var weeks = await get.ExecuteAsync(parameters, cancellationToken);
        var data = mapper.Map<CollectionResult<WeekModel>>(weeks);

        return data;
    }
}