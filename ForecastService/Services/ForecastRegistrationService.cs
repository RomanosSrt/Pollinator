using ForecastService.Contracts;
using ForecastService.Mapping;
using ForecastService.Helpers;
using ForecastService.Models.Pollinator.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YpenService.Services;

namespace ForecastService.Services
{
    public static class ForecastRegistrationService
    {
        public static void AddForecastServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpClient<IForecastClient, ForecastClient>();
            services.AddAutoMapper(cfg => cfg.AddProfile<ForecastMappingProfile>());
            services.AddScoped<IForecastDataService, ForecastDataService>();
            services.AddScoped<IForecastRepository, ForecastRepository>();
            services.Configure<OpenMeteoSettings>(config.GetSection("OpenMeteoSettings"));
            services.AddForecastPersistenceServices(config.GetConnectionString("DefaultConnection") ?? throw new Exception("Error on Forecast system configuration, connection string missing"));
        }
    }
}
