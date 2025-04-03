using System;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class DailyConfiguration : IEntityTypeConfiguration<Daily>
{
    public void Configure(EntityTypeBuilder<Daily> builder)
    {
        builder.HasMany(x => x.Forms).WithOne().OnDelete(DeleteBehavior.Cascade);

    }
}

