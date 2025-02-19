using System.Net;

namespace UniSchedule.Extensions.Exceptions.Base;

/// <summary>
///     Базовый класс для исключений в запросах
/// </summary>
public class RequestException : AggregateException
{
    /// <summary />
    protected RequestException(HttpStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }

    /// <summary />
    protected RequestException(HttpStatusCode statusCode, IEnumerable<Exception> innerExceptions) : base(
        innerExceptions)
    {
        StatusCode = statusCode;
    }

    /// <summary>
    ///     Статус ответа
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }
}