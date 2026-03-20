using API.Domain.Entities.PlotManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class PlotAvailabilityConfiguration : IEntityTypeConfiguration<PlotAvailability>
    {
        public void Configure(EntityTypeBuilder<PlotAvailability> builder)
        {

        }
    }
}
