using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services, string connectionstring)
        {
            // Register the DbContext with the connection string from configuration
            services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionstring, o => 
            { 
                o.UseNetTopologySuite();          
/*                o.EnableRetryOnFailure(
                    maxRetryCount: 0,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorCodesToAdd: null);
*/            }));
        }
    }
}
