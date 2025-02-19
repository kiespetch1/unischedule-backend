using System.Net;
using UniSchedule.Extensions.Exceptions.Base;

namespace UniSchedule.Extensions.Exceptions;

/// <summary>
///     Исключение "Нет прав доступа"
/// </summary>
public class NoAccessRightsException : RequestException
{
    /// <summary />
    public NoAccessRightsException() : base(HttpStatusCode.Forbidden, "Нет прав доступа") { }

    /// <summary />
    public NoAccessRightsException(string message) : base(HttpStatusCode.Forbidden, message) { }
}