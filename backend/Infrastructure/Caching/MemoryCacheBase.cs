using System.Diagnostics;

using Microsoft.Extensions.Caching.Memory;

using TaskSync.Infrastructure.Caching.Interfaces;

namespace TaskSync.Infrastructure.Caching
{
    public abstract class MemoryCacheBase<T> : IMemoryCacheService<T>
    {
        protected readonly IMemoryCache _memoryCache;
        protected MemoryCacheBase(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T?> GetAsync(int cacheKey, Func<Task<T?>> fallbackCall)
        {
            T? data;
            var sw = Stopwatch.StartNew();

            if (_memoryCache.TryGetValue(GetCacheKey(cacheKey), out T? cached))
            {
                Console.WriteLine($"[MemoryCacheBase] get '{GetCacheKey(cacheKey)}' from memory");
                data = cached;
            }
            else
            {
                Console.WriteLine($"[MemoryCacheBase] get '{GetCacheKey(cacheKey)}' from database");
                data = await fallbackCall();
                if (data != null) { Set(cacheKey, data); }
            }

            sw.Stop();
            Console.WriteLine($"[MemoryCacheBase] get '{GetCacheKey(cacheKey)}' {sw.ElapsedMilliseconds}ms");

            return data;
        }

        // todo-moch: make it customizable for expiration, size, etc.
        public void Set(int cacheKey, T data)
        {
            _memoryCache.Set(
                GetCacheKey(cacheKey),
                data,
                new MemoryCacheEntryOptions // simulate LRU behavior
                {
                    SlidingExpiration = TimeSpan.FromMinutes(10),
                    Size = 1,
                });
        }

        public void Remove(int cacheKey)
        {
            _memoryCache.Remove(GetCacheKey(cacheKey));
            Console.WriteLine($"[MemoryCacheBase] remove '{GetCacheKey(cacheKey)}' from memory");
        }

        protected abstract string GetCacheKey(int cacheKey);
    }
}
