using API.Domain.Entities.PlotManagement;
using API.Domain.Entities.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.fullName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.HasIndex(u => u.Email)
                   .IsUnique();

            //builder.HasIndex(u => u.UserName)             #TODO: Add on production
            //       .IsUnique();

            // UserType stored as int in the DB (0=BEEKEEPER, 1=FARMER, 2=ADMIN)
            builder.Property(u => u.userType)
                   .IsRequired()
                   .HasConversion<int>();
        }
    }
}
