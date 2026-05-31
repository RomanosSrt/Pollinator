using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using YpenService.Contracts;
using YpenService.Helpers;
using YpenService.Mapping;
using YpenService.Models.Pollinator.Settings;

namespace YpenService.Services
{
    public static class YpenRegistrationService
    {
        public static void AddYpenServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpClient<IYpenClient, YpenClient>();
            services.AddScoped<IYpenService, YpenService>();
            services.AddScoped<IUnitsRepository, UnitsRepository>();
            services.Configure<YpenSettings>(config.GetSection("YpenSettings"));
            services.Configure<DBSettings>(config.GetSection("ConnectionStrings"));
            services.AddAutoMapper(cfg => cfg.AddProfile<YpenMappingProfile>());
            services.AddYpenPersistenceServices(config.GetConnectionString("DefaultConnection") ?? throw new Exception("Error on YPEN system configuration, connection string missing"));
        }
    }
}
