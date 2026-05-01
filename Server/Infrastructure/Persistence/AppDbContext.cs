using Microsoft.EntityFrameworkCore;
using API.Domain.Entities.UserManagement;
using API.Domain.Entities.PlotManagement;
using API.Infrastructure.Persistence.Configurations;

namespace API.Infrastructure.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public required DbSet<Plot> Plots { get; set; }
        public required DbSet<Reservation> Reservations { get; set; }
        //public DbSet<User> Users { get; set; }
        public required DbSet<ApplicationUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new PlotConfiguration());
            //builder.ApplyConfiguration(new ReservationConfiguration());
            //builder.ApplyConfiguration(new PlotAvailabilityConfiguration());
            base.OnModelCreating(builder);
        }
        public static void EnsureDatabaseCreated(IServiceProvider services)
        {
            using var serviceScope = services.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.EnsureCreated();
        }

    }
}