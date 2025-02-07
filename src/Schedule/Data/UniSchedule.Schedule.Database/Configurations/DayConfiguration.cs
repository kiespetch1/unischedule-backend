using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniSchedule.Schedule.Entities;

namespace UniSchedule.Schedule.Database.Configurations;

/// <summary>
///     Конфигурация для дней
/// </summary>
public class DayConfiguration : IEntityTypeConfiguration<Day>
{
    /// <summary />
    public void Configure(EntityTypeBuilder<Day> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasMany(x => x.Classes)
            .WithOne(x => x.Day)
            .HasForeignKey(x => x.Id);
    }
}