using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniSchedule.Schedule.Entities;

namespace UniSchedule.Schedule.Database.Configurations;

/// <summary>
///     Конфигурация данных LMS
/// </summary>
public class LmsDataConfiguration : IEntityTypeConfiguration<LmsData>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<LmsData> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.Group)
            .WithMany(g => g.LmsData)
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}