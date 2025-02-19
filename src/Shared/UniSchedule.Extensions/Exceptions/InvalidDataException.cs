using System.Net;
using UniSchedule.Extensions.Exceptions.Base;

namespace UniSchedule.Extensions.Exceptions;

/// <summary>
///     Исключение "Неверные данные"
/// </summary>
public class InvalidDataException : RequestException
{
    /// <summary />
    public InvalidDataException() : base(HttpStatusCode.BadRequest, "Введенные данные некорректны") { }

    /// <summary />
    public InvalidDataException(string message) : base(HttpStatusCode.BadRequest, message) { }
}