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
///     Контроллер для работы с местами проведения
/// </summary>
[ApiController]
[Route("/api/v1/[controller]")]
public class LocationsController(
    ICreateCommand<Location, LocationCreateParameters, Guid> create,
    IDeleteCommand<Location, Guid> delete,
    IUpdateCommand<Location, LocationUpdateParameters, Guid> update,
    IMultipleQuery<Location, LocationQueryParameters> get,
    ISingleQuery<Location, Guid> getById,
    IMapper mapper)
{
    /// <summary>
    ///     Создание места проведения
    /// </summary>
    /// <param name="parameters">Параметры создания места проведения</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор места проведения</returns>
    /// <response code="200">Успешное создание места проведения</response>
    /// <response code="400">Некорректные данные</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPost]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.InternalServerError)]
    public async Task<Result<Guid>> CreateAsync(
        [FromBody] LocationCreateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var id = await create.ExecuteAsync(parameters, cancellationToken);

        return new Result<Guid>(id);
    }

    /// <summary>
    ///     Получение места проведения по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор места проведения</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Место проведения</returns>
    /// <response code="200">Успешное получение места проведения</response>
    /// <response code="404">Место проведения не найдено</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpGet("{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    public async Task<Result<LocationModel>> GetByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var location = await getById.ExecuteAsync(id, cancellationToken);
        var result = mapper.Map<LocationModel>(location);

        return new Result<LocationModel>(result);
    }

    /// <summary>
    ///     Получение списка мест проведения
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список мест проведения</returns>
    /// <response code="200">Успешное получение мест проведения</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpGet]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.InternalServerError)]
    public async Task<CollectionResult<LocationModel>> GetAsync(
        [FromQuery] LocationQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var locations = await get.ExecuteAsync(parameters, cancellationToken);
        var data = mapper.Map<CollectionResult<LocationModel>>(locations);

        return data;
    }

    /// <summary>
    ///     Обновление места проведения
    /// </summary>
    /// <param name="id">Идентификатор места проведения</param>
    /// <param name="parameters">Параметры обновления места проведения</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат операции</returns>
    /// <response code="200">Успешное обновление места проведения</response>
    /// <response code="400">Некорректные данные</response>
    /// <response code="404">Место проведения не найдено</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPut("{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    public async Task UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] LocationUpdateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        await update.ExecuteAsync(id, parameters, cancellationToken);
    }

    /// <summary>
    ///     Удаление места проведения
    /// </summary>
    /// <param name="id">Идентификатор места проведения</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат операции</returns>
    /// <response code="200">Успешное удаление места проведения</response>
    /// <response code="404">Место проведения не найдено</response>
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