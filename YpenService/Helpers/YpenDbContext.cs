using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;
using YpenService.Models.Pollinator.Persistence;

namespace YpenService.Helpers
{
    public class YpenDbContext(DbContextOptions<YpenDbContext> options) : DbContext(options)
    {
        public required DbSet<RegionUnits> RegionUnits { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RegionConfiguration());
            base.OnModelCreating(builder);
        }
        public static void EnsureDatabaseCreated(IServiceProvider services)
        {
            using var serviceScope = services.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<YpenDbContext>();
            context.Database.EnsureCreated();
        }
    }
}
