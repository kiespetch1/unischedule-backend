namespace UniSchedule.Extensions.Data;

/// <summary>
///     Ошибка
/// </summary>
public class Error
{
    /// <summary />
    public Error(Exception e)
    {
        switch (e)
        {
            case AggregateException ex:
                Message = ex.Message;
                InnerMessages = ex.InnerExceptions.Select(x => x.Message);
                break;
            default:
                Message = e.Message;
                break;
        }
    }

    /// <summary />
    public Error() { }

    /// <summary>
    ///     Сообщение об ошибке
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    ///     Вложенные сообщения об ошибках
    /// </summary>
    public IEnumerable<string>? InnerMessages { get; set; }
}