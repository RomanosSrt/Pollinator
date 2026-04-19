using API.Domain.Entities.PlotManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class PlotConfiguration : IEntityTypeConfiguration<Plot>
    {
        public void Configure(EntityTypeBuilder<Plot> builder)
        {
            builder.HasKey(p => p.PlotId);
            
            builder.Property(p => p.KAEK)
                   .IsRequired()
                   .HasMaxLength(12);

            builder.HasIndex(p => p.KAEK)
                   .IsUnique();

            builder.Property(p => p.Polygon)
                   .HasColumnType("geometry(Polygon, 4326)")
                   .IsRequired();

            builder.Property(p => p.Area)
                   .IsRequired();

            builder.Property(p => p.CropTypes)
                   .HasConversion(
                       v => string.Join(',', v),
                       v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                             .Select(x => Enum.Parse<CropType>(x))
                             .ToList()
                   );

            // 👨‍🌾 Ownership
            builder.Property(p => p.isClaimed)
                   .HasDefaultValue(false);

            builder.HasIndex(p => p.FarmerId);

            // 📅 Availabilities (1-N)
            builder.HasMany(p => p.Availabilities)
                .WithOne()
                .HasForeignKey(a => a.PlotId)
                .OnDelete(DeleteBehavior.Cascade);

            // 📦 Reservations (1-N)
            builder.HasMany(p => p.Reservations)
                   .WithOne()
                   .HasForeignKey(r => r.PlotId)
                   .OnDelete(DeleteBehavior.Cascade);

            // ⚡ Spatial Index (VERY IMPORTANT for thesis)
            builder.HasIndex(p => p.Polygon)
                   .HasMethod("GIST");
        }
    }
}
