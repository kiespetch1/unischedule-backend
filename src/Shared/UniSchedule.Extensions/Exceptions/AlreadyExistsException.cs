using System.Net;
using UniSchedule.Extensions.Exceptions.Base;

namespace UniSchedule.Extensions.Exceptions;

/// <summary>
///     Исключение "Уже существует"
/// </summary>
public class AlreadyExistsException : RequestException
{
    /// <summary />
    public AlreadyExistsException() : base(HttpStatusCode.BadRequest,
        "Введенные данные некорректны, объект с такими данными уже существует")
    {
    }

    /// <summary />
    public AlreadyExistsException(string message) : base(HttpStatusCode.BadRequest, message) { }
}