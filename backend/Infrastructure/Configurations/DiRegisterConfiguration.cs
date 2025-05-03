using TaskSync.Services;
using TaskSync.Repositories;
using TaskSync.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using TaskSync.Repositories.Interfaces;
using TaskSync.Repositories.Entities;
using TaskSync.Infrastructure.Http.Interface;
using TaskSync.Infrastructure.Http;
using TaskSync.Infrastructure.Caching;
using TaskSync.Infrastructure.Caching.Interfaces;

namespace TaskSync.Infrastructure.Configurations
{
    public static class DiRegisterConfiguration
    {
        public static IServiceCollection RegisterDependecies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddSingleton<IJwtService, JwtService>();
            services.AddSingleton<IMemoryCacheService<IList<TaskEntity>>, TaskEntityCache>();

            services.AddScoped<IHttpContextReader, HttpContextReader>();
            services.AddScoped<IRepository<UserEntity>, UserRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITaskService, TaskService>();

            return services;
        }
    }
}