using Microsoft.Extensions.Caching.Memory;

using TaskSync.Repositories.Entities;

namespace TaskSync.Infrastructure.Caching
{
    public class TaskEntityCache : MemoryCacheBase<IList<TaskEntity>>
    {
        public TaskEntityCache(IMemoryCache memorycache)
            : base(memorycache)
        {
        }

        protected override string GetCacheKey(int cacheKey) => $"tasks_project_{cacheKey}";
    }
}
