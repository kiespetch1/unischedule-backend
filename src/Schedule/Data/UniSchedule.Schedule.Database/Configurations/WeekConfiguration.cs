using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniSchedule.Schedule.Entities;

namespace UniSchedule.Schedule.Database.Configurations;

public class WeekConfiguration : IEntityTypeConfiguration<Week>
{
    /// <summary />
    public void Configure(EntityTypeBuilder<Week> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasMany(x => x.Days)
            .WithMany();
    }
}