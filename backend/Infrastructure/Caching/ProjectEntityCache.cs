using Microsoft.Extensions.Caching.Memory;

using TaskSync.Repositories.Entities;

namespace TaskSync.Infrastructure.Caching
{
    public class ProjectEntityCache : MemoryCacheBase<ProjectEntity>
    {
        public ProjectEntityCache(IMemoryCache memorycache)
            : base(memorycache)
        {
        }

        protected override string GetCacheKey(int cacheKey) => $"project_{cacheKey}";
    }
}
