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
using UniSchedule.Schedule.Entities.Owned;
using UniSchedule.Bot.Shared.Announcements;
using UniSchedule.Entities.DTO;

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
    public async Task<string> HandleEventAsync(VkEventParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var mappingResult = await VkEventMapper.Map(parameters, vkSettings, cancellationToken);
        var validationResult = mappingResult.Item1;

        if (!validationResult.IsValid)
        {
            var sb = new StringBuilder();
            sb.AppendJoin(" ", "Ошибка валидации события :");
            foreach (var error in validationResult.Errors)
            {
                sb.AppendJoin(" ", $"\t{error.PropertyName}: {error.ErrorMessage}");
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
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                };
                var eventJson = JsonSerializer.SerializeToDocument(@event, options);
                var message = eventJson.RootElement.GetProperty("object").GetProperty("message")
                    .Deserialize<VkMessage>(options);

                if (string.IsNullOrEmpty(message!.Text) || !message.Text.Contains("@all"))
                {
                    Log.Debug("{Message}", $"Не найдено ключевое слово. " +
                                           $"Событие: {eventJson.RootElement}");
                    return "ok";
                }

                var userMapping = await context.UserMessengerUser
                    .SingleOrDefaultAsync(x => x.MessengerUserId == message.FromId, cancellationToken);
                if (userMapping == null)
                {
                    Log.Debug("{Message}",
                        $"Пользователь с идентификатором {message.FromId} не найден в базе. " +
                        $"Событие: {eventJson.RootElement}");
                    return "ok";
                }

                var groupMapping = await context.GroupMessengerConversation
                    .SingleOrDefaultAsync(x => x.ConversationId == message.PeerId, cancellationToken);
                if (groupMapping == null)
                {
                    Log.Debug("{Message}", $"Группа с идентификатором {message.PeerId} не найдена в базе. " +
                                           $"Событие: {eventJson.RootElement}");
                    return "ok";
                }

                var userId = userMapping.UserId;
                var groupId = groupMapping.GroupId;
                var messageTrimmed = message.Text.Replace("@all", "").Trim();

                var data = new AnnouncementMqCreateParameters
                {
                    Message = messageTrimmed,
                    Target = new AnnouncementTargetInfo
                    {
                        IncludedGroups =
                            [groupId],
                        ExcludedGroups = [],
                        ExcludedDepartments = [],
                        ExcludedGrades = [],
                        IncludedDepartments = [],
                        IncludedGrades = []
                    },
                    IsAnonymous = false,
                    IsTimeLimited = false,
                    AvailableUntil = null,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    IsAddedUsingBot = true
                };
                await publisher.PublishAsync(data, cancellationToken);
                Log.Debug("{Message}", $"Отправлено объявление с текстом \"{messageTrimmed}\", " +
                                       $"для группы {groupId} от пользователя c идентификатором {userId}");

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
        await AddUserToConversationAsync(parameters.MessengerUserId, cancellationToken);
        await EnsureConversationLinkAsync(parameters.ConversationId, parameters.ConversationName, cancellationToken);
    }

    public Task<List<KeyValueItem<long>>> GetConversationsListAsync(CancellationToken cancellationToken = default)
    {
        var conversations = context.GroupMessengerConversation
            .AsQueryable()
            .Select(x => new KeyValueItem<long>(x.ConversationId, x.ConversationName))
            .ToListAsync(cancellationToken);

        return conversations;
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
    private async Task EnsureConversationLinkAsync(
        long conversationId,
        string conversationName,
        CancellationToken cancellationToken = default)
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
            GroupId = groupId, ConversationId = conversationId, ConversationName = conversationName
        });

        await context.SaveChangesAsync(cancellationToken);
    }
}