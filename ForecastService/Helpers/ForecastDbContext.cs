using ForecastService.Helpers;
using ForecastService.Models.Pollinator.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace YpenService.Helpers
{
    public class ForecastDbContext(DbContextOptions<ForecastDbContext> options) : DbContext(options)
    {
        public DbSet<AirQualityDAO> AirQuality { get; set; }
        public DbSet<WeatherDAO> Weather { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new WeatherConfiguration());
            builder.ApplyConfiguration(new AirQualityConfiguration());
            base.OnModelCreating(builder);
        }
        public static void EnsureDatabaseCreated(IServiceProvider services)
        {
            using var serviceScope = services.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<ForecastDbContext>();
            context.Database.EnsureCreated();
        }
    }
}
