using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    ILogger<EventService> logger,
    IUserContextProvider userContextProvider) : IEventService
{
    /// <inheritdoc/>
    public async Task<string> HandleEventAsync(VkEvent @event, CancellationToken cancellationToken = default)
    {
        var eventValidationResult = await VkEventValidator.Validate(@event, vkSettings, cancellationToken);
        if (!eventValidationResult.IsValid)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Ошибка валидации события :");
            foreach (var error in eventValidationResult.Errors)
            {
                sb.AppendLine($"\t{error.PropertyName}: {error.ErrorMessage}");
            }
            //TODO: логировать прям тут

            return "ok";
        }

        switch (@event.Type)
        {
            case VkResponseType.Confirmation:
                return vkSettings.ConfirmationCode;
            case VkResponseType.IncomingMessage:
                var message = JsonSerializer.SerializeToDocument(@event.Object!).Deserialize<Message>();

                if (string.IsNullOrEmpty(message!.Text) || !message.Text.Contains("@all"))
                {
                    // TODO: логировать такие неприятности
                    return "ok";
                }

                var userMapping = await context.UserMessengerUser
                    .SingleOrDefaultAsync(x => x.MessengerUserId == message.UserId, cancellationToken);
                if (userMapping == null)
                {
                    // TODO: логировать такие неприятности
                    return "ok";
                }

                var groupMapping = await context.GroupMessengerConversation
                    .SingleOrDefaultAsync(x => x.ConversationId == message.ChatId, cancellationToken);
                if (groupMapping == null)
                {
                    // TODO: логировать такие неприятности
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
                // TODO: логировать различную бесполезную нагрузку
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