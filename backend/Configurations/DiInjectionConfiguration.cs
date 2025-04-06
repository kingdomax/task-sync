using TaskSync.Services;
using TaskSync.Repositories;
using TaskSync.Repositories.Entity;
using TaskSync.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TaskSync.Configurations
{
    public static class DiInjectionConfiguration
    {
        public static IServiceCollection AddDiInjections(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            );

            // Repositories
            services.AddScoped<IRepository<UserEntity>, UserRepository>();

            // Services
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}