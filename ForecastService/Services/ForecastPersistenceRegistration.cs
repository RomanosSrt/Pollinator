using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YpenService.Helpers;

namespace YpenService.Services
{
    public static class ForecastPersistenceRegistration
    {
        public static void AddForecastPersistenceServices(this IServiceCollection services, string connectionstring)
        {
            // Register the DbContext with the connection string from configuration
            services.AddDbContext<ForecastDbContext>(options =>
            options.UseNpgsql(connectionstring, o =>
            {
                o.UseNetTopologySuite();
                /*                o.EnableRetryOnFailure(
                                    maxRetryCount: 0,
                                    maxRetryDelay: TimeSpan.FromSeconds(10),
                                    errorCodesToAdd: null);
                */
            }));
        }
    }
}
