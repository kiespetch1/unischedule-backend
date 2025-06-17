using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UniSchedule.Abstractions.Queries.Base;
using UniSchedule.Extensions.Attributes;
using UniSchedule.Extensions.Data;
using UniSchedule.Identity.Shared;
using UniSchedule.Identity.Shared.Attributes;
using UniSchedule.Schedule.Entities;
using UniSchedule.Schedule.Services.Abstractions;
using UniSchedule.Shared.DTO.Models;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Api.Controllers;

/// <summary>
///     Контроллер для работы с данными LMS
/// </summary>
[ApiController]
[Route("/api/v1/[controller]")]
public class LmsDataController(
    ILmsDataService service,
    IMultipleQuery<LmsData, LmsDataQueryParameters> get,
    ISingleQuery<LmsData, Guid> getById,
    IMapper mapper)
    : ControllerBase
{
    /// <summary>
    ///     Создание данных LMS
    /// </summary>
    [HttpPost]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin, RoleOption.GroupLeader, RoleOption.Staff)]
    public async Task<Result<Guid>> CreateAsync([
            FromBody]
        LmsDataCreateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var id = await service.CreateAsync(parameters, cancellationToken);

        return new Result<Guid>(id);
    }

    /// <summary>
    ///     Получение данных LMS по идентификатору
    /// </summary>
    [HttpGet("{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    public async Task<Result<LmsDataModel>> GetByIdAsync([
            FromRoute]
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await getById.ExecuteAsync(id, cancellationToken);
        var result = mapper.Map<LmsDataModel>(entity);

        return new Result<LmsDataModel>(result);
    }

    /// <summary>
    ///     Получение списка данных LMS
    /// </summary>
    [HttpGet]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.InternalServerError)]
    public async Task<CollectionResult<LmsDataModel>> GetAsync([
            FromQuery]
        LmsDataQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var entities = await get.ExecuteAsync(parameters, cancellationToken);
        var data = mapper.Map<CollectionResult<LmsDataModel>>(entities);

        return data;
    }

    /// <summary>
    ///     Обновление данных LMS
    /// </summary>
    [HttpPut("{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin, RoleOption.GroupLeader, RoleOption.Staff)]
    public async Task UpdateAsync([
            FromRoute]
        Guid id,
        [FromBody] LmsDataUpdateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        await service.UpdateAsync(id, parameters, cancellationToken);
    }

    /// <summary>
    ///     Удаление данных LMS
    /// </summary>
    [HttpDelete("{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin, RoleOption.GroupLeader, RoleOption.Staff)]
    public async Task DeleteAsync([
            FromRoute]
        Guid id,
        CancellationToken cancellationToken = default)
    {
        await service.DeleteAsync(id, cancellationToken);
    }
}