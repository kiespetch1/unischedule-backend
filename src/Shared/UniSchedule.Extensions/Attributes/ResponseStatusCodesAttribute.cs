using System.Net;

namespace UniSchedule.Extensions.Attributes;

/// <summary>
///     Атрибут для генерации схемы ответа для заданных статус кодов и типа результата
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class ResponseStatusCodesAttribute : Attribute
{
    /// <summary>
    ///     Создает новый экземпляр <see cref="ResponseStatusCodesAttribute" /> с заданными статус кодами
    /// </summary>
    /// <param name="statusCodes">Статус коды</param>
    public ResponseStatusCodesAttribute(params HttpStatusCode[] statusCodes)
    {
        StatusCodes = statusCodes;
    }

    /// <summary>
    ///     Статус коды, которые должны отображаться в Swagger
    /// </summary>
    public HttpStatusCode[] StatusCodes { get; }
}