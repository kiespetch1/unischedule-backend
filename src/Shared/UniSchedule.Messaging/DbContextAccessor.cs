using Microsoft.EntityFrameworkCore;

namespace UniSchedule.Messaging;

/// <inheritdoc />
public class DbContextAccessor<TContext>(TContext dbContext) : IDbContextAccessor
    where TContext : DbContext
{
    public DbContext GetDbContext()
    {
        return dbContext;
    }
}