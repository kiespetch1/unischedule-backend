using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace UniSchedule.Bot.Entities.Vk;

/// <summary>
///     Варианты ответа от VK API
/// </summary>
/// 
[JsonConverter(typeof(StringEnumConverter))]  
public enum VkResponseType
{
    /// <summary>
    ///     Подтверждение доступа
    /// </summary>
    [EnumMember(Value = "confirmation")]
    Confirmation = 0,
    
    /// <summary>
    ///     Входящее сообщение
    /// </summary>
    [EnumMember(Value = "message_new")]
    IncomingMessage = 1,
    
    /// <summary>
    ///     Исходящее сообщение
    /// </summary>
    [EnumMember(Value = "message_reply")]
    OutgoingMessage = 2,
    
    /// <summary>
    ///     Редактирование сообщения
    /// </summary>
    [EnumMember(Value = "message_edit")]
    MessageEdit = 3
}