using Microsoft.Extensions.Caching.Memory;

using TaskSync.Repositories.Entities;

namespace TaskSync.Infrastructure.Caching
{
    public class TaskEntitiesCache : MemoryCacheBase<IList<TaskEntity>>
    {
        public TaskEntitiesCache(IMemoryCache memorycache) : base(memorycache) { }

        protected override string GetCacheKey(int cacheKey) => $"tasks_project_{cacheKey}";
    }
}
