using System.Net;

namespace UniSchedule.Extensions.Exceptions.Base;

/// <summary>
///     Исключение "Не найдено"
/// </summary>
/// <param name="message">Сообщение об ошибке</param>
public class NotFoundException(string message) : RequestException(HttpStatusCode.NotFound, message);

/// <summary>
///     Типизированное исключение "Не найдено" для сущностей
/// </summary>
/// <param name="id">Идентификатор сущности</param>
/// <typeparam name="TEntity">Тип сущности</typeparam>
public class NotFoundException<TEntity>(string id)
    : NotFoundException($"{typeof(TEntity).Name} с идентификатором {id} не найден");