using System.Net;
using System.Reflection;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using R.Tech.Extensions.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;
using UniSchedule.Extensions.Basic;
using UniSchedule.Extensions.Data;

namespace UniSchedule.Extensions.DI.Swagger;

/// <summary>
///     Пользовательский фильтр ошибок
/// </summary>
public class StatusCodeFilter : IOperationFilter
{
    /// <summary>
    ///     Добавление схемы ответа для заданных статус кодов
    /// </summary>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var resultType = context.MethodInfo.ReturnType;

        var responseAttribute = context.MethodInfo.GetCustomAttribute<ResponseStatusCodesAttribute>()
                                ?? context.MethodInfo.DeclaringType?.GetCustomAttribute<ResponseStatusCodesAttribute>();

        if (responseAttribute == null)
        {
            ApplySchema(
                operation,
                HttpStatusCode.InternalServerError,
                GenerateSchema(context, typeof(Result<>).MakeGenericType(typeof(object))),
                context);
            ApplySchema(
                operation,
                HttpStatusCode.OK,
                GenerateSchema(context, resultType != typeof(Result<>) ||
                                        resultType != typeof(CollectionResult<>)
                    ? null
                    : resultType),
                context);
            return;
        }

        foreach (var statusCode in responseAttribute.StatusCodes)
        {
            resultType = GetResultType(context.MethodInfo.ReturnType, statusCode);
            var schema = GenerateSchema(context, resultType);
            ApplySchema(operation, statusCode, schema, context);
        }
    }

    private static Type? GetResultType(Type? initialResultType, HttpStatusCode statusCode)
    {
        switch (statusCode.ToInt32())
        {
            case >= 200 and < 300:
                if (initialResultType == typeof(Task) ||
                    initialResultType == typeof(void) ||
                    initialResultType == typeof(Task<FileStreamResult>))
                {
                    return null;
                }

                break;
            case >= 400 and < 600:
                return typeof(Result<>).MakeGenericType(typeof(object));
        }

        return initialResultType;
    }

    private static OpenApiSchema? GenerateSchema(OperationFilterContext context, Type? resultType)
    {
        return resultType == null ? null : context.SchemaGenerator.GenerateSchema(resultType, context.SchemaRepository);
    }

    private static void ApplySchema(
        OpenApiOperation operation,
        HttpStatusCode statusCode,
        OpenApiSchema? schema,
        OperationFilterContext context)
    {
        const string jsonSchema = "application/json";
        const string streamSchema = "application/octet-stream";

        var isSuccessStatusCode = statusCode.ToInt32() >= 200 && statusCode.ToInt32() < 300;

        var currentSchema = isSuccessStatusCode && context.MethodInfo.ReturnType == typeof(Task<FileStreamResult>)
            ? streamSchema
            : jsonSchema;

        if (!operation.Responses.TryGetValue(statusCode.ToInt32().ToString(), out var response))
        {
            response = new OpenApiResponse { Description = statusCode.GetDescription().Humanize() };
            operation.Responses.Add(statusCode.ToInt32().ToString(), response);
        }

        if (schema == null)
        {
            response.Content = null;
        }

        response.Content ??= new Dictionary<string, OpenApiMediaType>();

        if (isSuccessStatusCode)
        {
            if (response.Content.TryGetValue(currentSchema, out var mediaType))
            {
                mediaType.Schema = schema;
            }
            else
            {
                mediaType = new OpenApiMediaType { Schema = schema };
                response.Content.Add(currentSchema, mediaType);
            }
        }
        else
        {
            response.Content.Clear();
            response.Content.Add(jsonSchema, new OpenApiMediaType { Schema = schema });
        }
    }
}