using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using YpenService.Models.Pollinator.Persistence;

namespace YpenService.Helpers
{
    public class RegionConfiguration : IEntityTypeConfiguration<RegionUnit>
    {
        public void Configure(EntityTypeBuilder<RegionUnit> builder) {
            builder.HasKey(r => r.unit_KALCODE);

            builder.Property(r => r.unit_KALCODE)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(r => r.unit_Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(r => r.unit_Center)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(r => r.unit_Latitude)
                .IsRequired();

            builder.Property(r => r.unit_Longitude)
                .IsRequired();

            builder.Property(r => r.unit_Shapes)
                .HasColumnType("geometry(MultiPolygon, 4326)")  // PostgreSQL multi-polygon type with SRID 4326 to store rings, islands, and holes
                .IsRequired();

            builder.Property(r => r.unit_Area)
                .IsRequired();
        }
    }
}
