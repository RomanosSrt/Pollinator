using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using YpenService.Helpers;

namespace YpenService.Services
{
    public static class YpenPersistenceRegistration
    {
        public static void AddYpenPersistenceServices(this IServiceCollection services, string connectionstring)
        {
            // Register the DbContext with the connection string from configuration
            services.AddDbContext<YpenDbContext>(options =>
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
