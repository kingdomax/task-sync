using Microsoft.Extensions.Caching.Memory;
using TaskSync.Infrastructure.Caching.Interfaces;

namespace TaskSync.Infrastructure.Caching
{
    public abstract class MemoryCacheBase<T> : IMemoryCacheService<T>
    {
        protected readonly IMemoryCache _memoryCache;
        protected MemoryCacheBase(IMemoryCache memoryCache) => _memoryCache = memoryCache;
        
        protected abstract string GetCacheKey(int cacheKey);

        public async Task<T?> GetAsync(int cacheKey, Func<Task<T?>> fallbackCall)
        {
            if (_memoryCache.TryGetValue(GetCacheKey(cacheKey), out T? cached))
            {
                Console.WriteLine($"[MemoryCache] get '{GetCacheKey(cacheKey)}' from memory");
                return cached;
            }

            var data = await fallbackCall();
            if (data != null)
            {
                Set(cacheKey, data);
            }
            Console.WriteLine($"[MemoryCache] get '{GetCacheKey(cacheKey)}' from database");

            return data;
        }

        public void Set(int cacheKey, T data)
        {
            _memoryCache.Set(
                GetCacheKey(cacheKey),
                data,
                new MemoryCacheEntryOptions // simulate LRU behavior
                {
                    SlidingExpiration = TimeSpan.FromMinutes(10),
                    Size = 1
                }
            );
        }

        public void Remove(int cacheKey)
        {
            _memoryCache.Remove(GetCacheKey(cacheKey));
            Console.WriteLine($"[MemoryCache] remove '{GetCacheKey(cacheKey)}' from memory");
        }
    }
}
