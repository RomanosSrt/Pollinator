using ForecastService.Contracts;
using ForecastService.Models.Pollinator.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ForecastService.Services
{
    public static class ForecastRegistrationService
    {
        public static void AddForecastServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpClient<IForecastClient, ForecastClient>();
            services.AddScoped<IForecastDataService, ForecastDataService>();
            services.Configure<OpenMeteoSettings>(config.GetSection("OpenMeteoSettings"));
        }
    }
}
