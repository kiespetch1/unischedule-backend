using System.Text.Json;
using FluentValidation.Results;
using Serilog;
using UniSchedule.Bot.Entities.Settings;
using UniSchedule.Bot.Entities.Vk;
using UniSchedule.Bot.Shared;
using UniSchedule.Extensions.Basic;

namespace UniSchedule.Bot.Services;

/// <summary>
///     Маппер события из VK
/// </summary>
public class VkEventMapper
{
    public static async Task<(ValidationResult, VkEvent?)> Map(
        VkEventParameters parameters,
        VkApiSettings vkSettings,
        CancellationToken cancellationToken)
    {
        var result = new ValidationResult();
        var confirmationType = VkResponseType.Confirmation.GetMemberValue();
        var incomingMessageType = VkResponseType.IncomingMessage.GetMemberValue();
        var outgoingMessageType = VkResponseType.OutgoingMessage.GetMemberValue();
        var messageEditType = VkResponseType.MessageEdit.GetMemberValue();
        Log.Debug("Начало обработки события: {@Parameters}", parameters);

        if (parameters.Type == confirmationType && parameters.Object is not null)
        {
            result.Errors.Add(new ValidationFailure(nameof(parameters.Object),
                "Неверный формат объекта для события \"Подтверждение\""));
        }

        if (parameters.Type == confirmationType && parameters.Object is null)
        {
            return (result, new VkEvent{Type = VkResponseType.Confirmation});
        }

        if (parameters.Version != vkSettings.ApiVersion)
        {
            result.Errors.Add(new ValidationFailure(nameof(parameters.Version), "Неподдерживаемая версия VK API"));

            return (result, null);
        }
        
        if (parameters.GroupId.ToString() != vkSettings.GroupId)
        {
            result.Errors.Add(new ValidationFailure(nameof(parameters.GroupId), "Неверный идентификатор группы VK"));

            return (result, null);
        }
        
        if (parameters.Secret != vkSettings.Secret)
        {
            result.Errors.Add(new ValidationFailure(nameof(parameters.Secret), "Неверный секрет VK"));

            return (result, null);
        }

        var expectedFileName = $"{parameters.Type}.json";

        var filePath = Directory
            .GetFiles(vkSettings.ResponseObjectTemplatesPath, "*.json")
            .SingleOrDefault(fullPath =>
                Path.GetFileName(fullPath)
                    .Equals(expectedFileName, StringComparison.OrdinalIgnoreCase));

        if (filePath == null)
        {
            throw new FileNotFoundException(
                $"Шаблон для события '{parameters.Type}' не найден в папке '{vkSettings.ResponseObjectTemplatesPath}'.");
        }

        using var template = JsonDocument.Parse(await File.ReadAllTextAsync(filePath, cancellationToken));
        var @event = new VkEvent();

        switch (parameters.Type)
        {
            case var t when t == incomingMessageType:
                if (parameters.Object == null || !parameters.Object.Validate(template))
                {
                    result.Errors.Add(new ValidationFailure(nameof(parameters.Object),
                        "Неверный формат объекта для события \"Входящее сообщение\""));
                }
                else
                {
                    @event.Type = VkResponseType.IncomingMessage;
                    @event.Object = parameters.Object.ToJsonDocument();
                }

                break;
            case var t when t == outgoingMessageType:
                //TODO: реализовать валидацию этого типа
                result.Errors.Add(new ValidationFailure(nameof(parameters.Object),"Неподдерживаемый тип события"));
                break;
            case var t when t == messageEditType:
                //TODO: реализовать валидацию этого типа
                result.Errors.Add(new ValidationFailure(nameof(parameters.Object),"Неподдерживаемый тип события"));
                break;
        }

        @event.GroupId = parameters.GroupId;
        @event.Version = parameters.Version;

        return (result, @event);
    }
}