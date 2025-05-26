using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Serilog;
using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Bot.Database;
using UniSchedule.Bot.Entities.Auxiliary;
using UniSchedule.Bot.Entities.Settings;
using UniSchedule.Bot.Entities.Vk;
using UniSchedule.Bot.Services.Abstractions;
using UniSchedule.Bot.Shared;
using VkNet.Model;
using UniSchedule.Schedule.Entities.Owned;
using UniSchedule.Bot.Shared.Announcements;

namespace UniSchedule.Bot.Services;

/// <summary>
///     Сервис обработки событий из мессенджеров
/// </summary>
public class EventService(
    DatabaseContext context,
    VkApiSettings vkSettings,
    IPublisher<AnnouncementMqCreateParameters> publisher,
    IUserContextProvider userContextProvider) : IEventService
{
    /// <inheritdoc/>
    public async Task<string> HandleEventAsync(VkEventParameters parameters, CancellationToken cancellationToken = default)
    {
        var mappingResult = await VkEventMapper.Map(parameters, vkSettings, cancellationToken);
        var validationResult = mappingResult.Item1;
        
        if (validationResult.IsValid)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Ошибка валидации события :");
            foreach (var error in validationResult.Errors)
            {
                sb.AppendLine($"\t{error.PropertyName}: {error.ErrorMessage}");
            }

            Log.Error("{Message}", $"{sb} Событие: {parameters}");

            return "ok";
        }
        var @event = mappingResult.Item2!;

        switch (@event.Type)
        {
            case VkResponseType.Confirmation:
                return vkSettings.ConfirmationCode;
            case VkResponseType.IncomingMessage:
                var message = JsonSerializer.SerializeToDocument(@event.Object!).Deserialize<Message>();

                if (string.IsNullOrEmpty(message!.Text) || !message.Text.Contains("@all"))
                {
                    Log.Debug("{Message}", $"Не найдено ключевое слово. Событие: {parameters}");
                    return "ok";
                }

                var userMapping = await context.UserMessengerUser
                    .SingleOrDefaultAsync(x => x.MessengerUserId == message.UserId, cancellationToken);
                if (userMapping == null)
                {
                    Log.Debug("{Message}",
                        $"Пользователь с идентификатором {message.UserId} не найден в базе. Событие: {parameters}");
                    return "ok";
                }

                var groupMapping = await context.GroupMessengerConversation
                    .SingleOrDefaultAsync(x => x.ConversationId == message.ChatId, cancellationToken);
                if (groupMapping == null)
                {
                    Log.Debug("{Message}",
                        $"Группа с идентификатором {message.ChatId} не найдена в базе. Событие: {parameters}");
                    return "ok";
                }

                var userId = userMapping.UserId;
                var groupId = groupMapping.GroupId;

                var data = new AnnouncementMqCreateParameters
                {
                    Message = message.Text,
                    Target = new AnnouncementTargetInfo { IncludedGroups = [groupId] },
                    IsAnonymous = false,
                    IsTimeLimited = false,
                    AvailableUntil = null,
                    CreatedBy = userId,
                    IsAddedUsingBot = true
                };
                await publisher.PublishAsync(data, cancellationToken);

                return "ok";

            case VkResponseType.MessageEdit:
            case VkResponseType.OutgoingMessage:
            default:
                Log.Debug("{Message}", $"Неподдерживаемый тип события: {parameters}");
                return "ok";
        }
    }

    /// <inheritdoc/>
    public async Task LinkMessengerAsync(MessengerLinkParameters parameters, CancellationToken cancellationToken)
    {
        await EnsureConversationLinkAsync(parameters.ConversationId, cancellationToken);
        await AddUserToConversationAsync(parameters.MessengerUserId, cancellationToken);
    }

    /// <summary>
    ///     Добавление прав на создание уведомления из бота для пользователя
    /// </summary>
    /// <param name="messengerUserId">Идентификатор пользователя в мессенджере</param>
    /// <param name="cancellationToken">Токен отмены</param>
    private async Task AddUserToConversationAsync(long messengerUserId, CancellationToken cancellationToken)
    {
        var userId = userContextProvider.GetContext().Id;
        context.UserMessengerUser.Add(new UserMessengerUser { UserId = userId, MessengerUserId = messengerUserId });
        
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    ///     Проверка на наличие записи для текущей группы
    /// </summary>
    private async Task EnsureConversationLinkAsync(long conversationId, CancellationToken cancellationToken)
    {
        var isLinked = await context.GroupMessengerConversation
            .AnyAsync(x => x.ConversationId == conversationId, cancellationToken);
        if (isLinked)
        {
            return;
        }

        var groupId = userContextProvider.GetContext().GroupId;
        context.GroupMessengerConversation.Add(new GroupMessengerConversation
        {
            GroupId = groupId, ConversationId = conversationId
        });
        
        await context.SaveChangesAsync(cancellationToken);
    }
}