namespace TaskSync.Infrastructure.Caching.Interfaces
{
    public interface IMemoryCacheService<T>
    {
        Task<T?> GetAsync(int cacheKey, Func<Task<T?>> fallbackCall);
        void Set(int cacheKey, T data);
        void Remove(int cacheKey);
    }
}
