using TaskSync.Services;
using TaskSync.Repositories;
using TaskSync.Repositories.Entity;
using TaskSync.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using TaskSync.Repositories.Interfaces;

namespace TaskSync.Configurations
{
    public static class DiRegisterConfiguration
    {
        public static IServiceCollection RegisterDependecies(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            );

            // Repositories
            services.AddScoped<IRepository<UserEntity>, UserRepository>();

            // Services
            services.AddSingleton<IJwtService, JwtService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITaskService, TaskService>();

            return services;
        }
    }
}