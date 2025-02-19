using System.Net;
using UniSchedule.Extensions.Exceptions.Base;

namespace UniSchedule.Extensions.Exceptions;

/// <summary>
///     Исключение при валидации данных объекта
/// </summary>
public class ValidationException : RequestException
{
    /// <summary />
    public ValidationException(IEnumerable<System.IO.InvalidDataException> exceptions) : base(HttpStatusCode.BadRequest,
        exceptions)
    {
    }
}