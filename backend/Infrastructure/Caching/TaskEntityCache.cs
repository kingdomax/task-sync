using TaskSync.Repositories.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace TaskSync.Infrastructure.Caching
{
    public class TaskEntityCache : MemoryCacheBase<IList<TaskEntity>>
    {
        public TaskEntityCache(IMemoryCache memorycache) : base(memorycache) { } // todo: I don't know can I inject like this, or can I move DI to abstract class

        protected override string GetCacheKey(int cacheKey) => $"tasks_project_{cacheKey}";
    }
}
