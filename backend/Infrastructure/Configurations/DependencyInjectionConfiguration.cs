using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using TaskSync.ExternalApi;
using TaskSync.ExternalApi.Interfaces;
using TaskSync.Infrastructure.Caching;
using TaskSync.Infrastructure.Caching.Interfaces;
using TaskSync.Infrastructure.Http;
using TaskSync.Infrastructure.Http.Interface;
using TaskSync.Infrastructure.Settings;
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
        public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(provider.GetRequiredService<IOptions<PostgreSqlSettings>>().Value.DefaultConnection)
            );

            services.AddSingleton<IJwtService, JwtService>();
            services.AddSingleton<IHomeVmService, HomeVmService>();
            services.AddSingleton<ITaskNotificationService, TaskNotificationService>();
            services.AddSingleton<ICacheBackgroundRefresher, CacheBackgroundRefresher>();
            services.AddSingleton<IMemoryCacheService<IList<TaskEntity>>, TaskEntityCache>();
            services.AddSingleton<IMemoryCacheService<ProjectEntity>, ProjectEntityCache>();

            services.AddScoped<IHttpContextReader, HttpContextReader>();
            services.AddScoped<IRepository<UserEntity>, UserRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ICommentService, CommentService>();

            services.AddScoped<IGamificationApi, GamificationApi>();

            return services;
        }
    }
}
