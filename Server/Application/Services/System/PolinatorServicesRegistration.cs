using API.Application.Services.Implementation;
using API.Application.Services.Interfaces;
using API.Infrastructure.Repositories.Implementation;
using API.Infrastructure.Repositories.Interfaces;

namespace API.Application.Services.System
{
    public static class PolinatorServicesRegistration
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            Console.WriteLine("Registering services...");
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            //services.AddAutoMapper(typof(MappingProfile));
        }
    }
}
