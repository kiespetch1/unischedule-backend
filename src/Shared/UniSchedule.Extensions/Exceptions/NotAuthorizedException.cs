using System.Net;
using UniSchedule.Extensions.Exceptions.Base;

namespace UniSchedule.Extensions.Exceptions;

/// <summary>
///     Исключение "Не авторизован"
/// </summary>
public class NotAuthorizedException : RequestException
{
    /// <summary />
    public NotAuthorizedException() : base(HttpStatusCode.Unauthorized, "Не авторизован") { }

    /// <summary />
    public NotAuthorizedException(string message) : base(HttpStatusCode.Unauthorized, message) { }
}