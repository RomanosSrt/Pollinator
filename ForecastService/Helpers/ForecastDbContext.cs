using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace YpenService.Helpers
{
    public class ForecastDbContext(DbContextOptions<ForecastDbContext> options) : DbContext(options)
    {

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.ApplyConfiguration(new RegionConfiguration());
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
