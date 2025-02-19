using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniSchedule.Schedule.Entities;

namespace UniSchedule.Schedule.Database.Configurations;

/// <summary>
///     Конфигурация для объявлений
/// </summary>
public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
{
    /// <summary />
    public void Configure(EntityTypeBuilder<Announcement> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Target, owned =>
        {
            owned.ToJson();
        });
    }
}