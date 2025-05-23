using System.Text.Json;
using FluentValidation.Results;
using Humanizer;
using UniSchedule.Bot.Entities.Settings;
using UniSchedule.Bot.Entities.Vk;
using UniSchedule.Extensions.Basic;

namespace UniSchedule.Bot.Services;

/// <summary>
///     Валидатор события из VK
/// </summary>
public class VkEventValidator
{
    public static async Task<ValidationResult> Validate(
        VkEvent @event,
        VkApiSettings vkSettings,
        CancellationToken cancellationToken)
    {
        var result = new ValidationResult();

        if (@event is { Type: VkResponseType.Confirmation, Object: not null })
        {
            result.Errors.Add(new ValidationFailure(nameof(@event.Object),
                "Неверный формат объекта для события \"Подтверждение\""));
        }

        if (@event is { Type: VkResponseType.Confirmation, Object: null })
        {
            return result;
        }

        if (@event.Version != vkSettings.Version)
        {
            result.Errors.Add(new ValidationFailure(nameof(@event.Version), "Неподдерживаемая версия VK API"));

            return result;
        }

        var eventTypeName = @event.Type.ToString().Underscore();
        var expectedFileName = $"{eventTypeName}.json";

        var filePath = Directory
            .GetFiles(vkSettings.ResponseObjectTemplatesPath, "*.json")
            .SingleOrDefault(fullPath =>
                Path.GetFileName(fullPath)
                    .Equals(expectedFileName, StringComparison.OrdinalIgnoreCase));

        if (filePath == null)
        {
            throw new FileNotFoundException(
                $"Шаблон для события '{eventTypeName}' не найден в папке '{vkSettings.ResponseObjectTemplatesPath}'.");
        }

        var json = await File.ReadAllTextAsync(filePath, cancellationToken);
        using var data = JsonDocument.Parse(json);

        switch (@event.Type)
        {
            case VkResponseType.IncomingMessage:
                if (@event.Object == null || !@event.Object.Validate(data))
                {
                    result.Errors.Add(new ValidationFailure(nameof(@event.Object),
                        "Неверный формат объекта для события \"Входящее сообщение\""));
                }

                break;
            case VkResponseType.OutgoingMessage:
            case VkResponseType.MessageEdit:
                // TODO: доделать потом
                break;
        }

        return result;
    }
}