﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniSchedule.Schedule.Entities;

namespace UniSchedule.Schedule.Database.Configurations;

/// <summary>
///     Конфигурация для пар
/// </summary>
public class ClassConfiguration : IEntityTypeConfiguration<Class>
{
    /// <summary />
    public void Configure(EntityTypeBuilder<Class> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.Day)
            .WithMany(x => x.Classes)
            .HasForeignKey(x => x.DayId);

        builder
            .HasOne(x => x.Location)
            .WithMany(x => x.Classes)
            .HasForeignKey(x => x.LocationId);

        builder
            .HasOne(x => x.Teacher)
            .WithMany(x => x.Classes)
            .HasForeignKey(x => x.TeacherId);
    }
}