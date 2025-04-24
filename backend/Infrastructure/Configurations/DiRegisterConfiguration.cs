using TaskSync.Services;
using TaskSync.Repositories;
using TaskSync.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using TaskSync.Repositories.Interfaces;
using TaskSync.Repositories.Entities;
using TaskSync.Infrastructure.Http.Interface;
using TaskSync.Infrastructure.Http;

namespace TaskSync.Infrastructure.Configurations
{
    public static class DiRegisterConfiguration
    {
        public static IServiceCollection RegisterDependecies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IHttpContextReader, HttpContextReader>();
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            );

            // Repositories
            services.AddScoped<IRepository<UserEntity>, UserRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();

            // Services
            services.AddSingleton<IJwtService, JwtService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITaskService, TaskService>();

            return services;
        }
    }
}