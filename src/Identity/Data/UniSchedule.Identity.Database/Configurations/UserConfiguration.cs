using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniSchedule.Identity.Entities;

namespace UniSchedule.Identity.Database.Configurations;

/// <summary>
///     Конфигурация для пользователей
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary />
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Password);
        
        builder.HasOne(x => x.Group);
    }
}