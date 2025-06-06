using TaskSync.Infrastructure.Caching.Interfaces;
using TaskSync.Repositories.Entities;
using TaskSync.Repositories.Interfaces;

namespace TaskSync.Infrastructure.Caching
{
    public class CacheBackgroundRefresher : ICacheBackgroundRefresher
    {
        private readonly IServiceProvider _sp;

        public CacheBackgroundRefresher(IServiceProvider sp)
        {
            _sp = sp;
        }

        public void RefreshProjectTasks(int projectId)
        {
            // Explicitly queue in the thread pool.
            Task.Run(async () =>
            {
                using var scope = _sp.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
                var cache = _sp.GetRequiredService<IMemoryCacheService<IList<TaskEntity>>>();

                var tasks = await repo.GetAsync(projectId);
                if (tasks != null)
                {
                    cache.Set(projectId, tasks);
                }
            });
        }
    }
}
