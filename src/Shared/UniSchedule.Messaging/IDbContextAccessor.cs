using Microsoft.EntityFrameworkCore;

namespace UniSchedule.Messaging;

/// <summary>
///     Интерфейс для предоставления доступа к <see cref="DbContext" />
/// </summary>
public interface IDbContextAccessor
{
    DbContext GetDbContext();
}