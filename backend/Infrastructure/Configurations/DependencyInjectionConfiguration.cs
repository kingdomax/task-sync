using Microsoft.EntityFrameworkCore;

using TaskSync.ExternalApi;
using TaskSync.ExternalApi.Interfaces;
using TaskSync.Infrastructure.Caching;
using TaskSync.Infrastructure.Caching.Interfaces;
using TaskSync.Infrastructure.Http;
using TaskSync.Infrastructure.Http.Interface;
using TaskSync.Repositories;
using TaskSync.Repositories.Entities;
using TaskSync.Repositories.Interfaces;
using TaskSync.Services;
using TaskSync.Services.Interfaces;
using TaskSync.SignalR;
using TaskSync.SignalR.Interfaces;

namespace TaskSync.Infrastructure.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddSingleton<IJwtService, JwtService>();
            services.AddSingleton<ITaskNotificationService, TaskNotificationService>();
            services.AddSingleton<IMemoryCacheService<IList<TaskEntity>>, TaskEntitiesCache>();
            services.AddSingleton<ICacheBackgroundRefresher, CacheBackgroundRefresher>();

            services.AddScoped<IHttpContextReader, HttpContextReader>();
            services.AddScoped<IRepository<UserEntity>, UserRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITaskService, TaskService>();

            services.AddScoped<IGamificationApi, GamificationApi>();

            return services;
        }
    }
}
