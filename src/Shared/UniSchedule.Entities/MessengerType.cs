namespace UniSchedule.Entities;

/// <summary>
///     Используемый мессенджер
/// </summary>
public enum MessengerType
{
   /// <summary>
   ///     Не указан
   /// </summary>
    None = 0,
    
   /// <summary>
   ///     ВКонтакте
   /// </summary>
    Vk = 1,
    
   /// <summary>
   ///     Telegram
   /// </summary>
    Telegram = 2,
}