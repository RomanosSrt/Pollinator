using API.Domain.Entities.PlotManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class PlotConfiguration : IEntityTypeConfiguration<Plot>
    {
        public void Configure(EntityTypeBuilder<Plot> builder)
        {
            builder.HasKey(p => p.plotId);
            
            builder.Property(p => p.kaek)
                   .HasMaxLength(12);

            builder.HasIndex(p => p.kaek)
                   .IsUnique();

            builder.Property(p => p.polygon)
                   .HasColumnType("geometry(polygon, 4326)")
                   .IsRequired();

            builder.Property(p => p.area)
                   .IsRequired();

            builder.Property(p => p.cropTypes)
                   .HasConversion<int>();

            // 👨‍🌾 Ownership
            builder.Property(p => p.isClaimed)
                   .HasDefaultValue(false);

            builder.HasIndex(p => p.farmerId);

            // 📅 availabilities (1-N)
            builder.HasMany(p => p.availabilities)
                .WithOne()
                .HasForeignKey(a => a.PlotId)
                .OnDelete(DeleteBehavior.Cascade);

            // 📦 reservations (1-N)
            builder.HasMany(p => p.reservations)
                   .WithOne()
                   .HasForeignKey(r => r.PlotId)
                   .OnDelete(DeleteBehavior.Cascade);

            // ⚡ Spatial Index (VERY IMPORTANT for thesis)
            builder.HasIndex(p => p.polygon)
                   .HasMethod("GIST");
        }
    }
}
