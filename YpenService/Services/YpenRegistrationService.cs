using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.ResponseCompression;
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
            services.AddHttpClient<IYpenClient, YpenClient>().ConfigureHttpClient(client =>
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
            });
            services.AddScoped<IYpenService, YpenService>();
            services.AddScoped<IUnitsRepository, UnitsRepository>();
            services.Configure<YpenSettings>(config.GetSection("YpenSettings"));
            services.Configure<DBSettings>(config.GetSection("ConnectionStrings"));
            services.AddAutoMapper(cfg => cfg.AddProfile<YpenMappingProfile>());
            services.AddYpenPersistenceServices(config.GetConnectionString("DefaultConnection") ?? throw new Exception("Error on YPEN system configuration, connection string missing"));

            #region response optimization
            services.AddResponseCompression(opts =>
            {
                opts.Providers.Add<GzipCompressionProvider>();  //compress response for big byte streams automatical decompression from browser
            });
            services.AddMemoryCache();      //use cache instead of hitting the database for every request
            #endregion
        }
    }
}
