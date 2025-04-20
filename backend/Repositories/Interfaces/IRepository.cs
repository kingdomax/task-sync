namespace TaskSync.Repositories.Interfaces
{
    // Segregate interface (SOLID principle)
    public interface IRepository<T>
    {
        public Task<T?> GetAsync();
        public Task<T?> GetAsync(string param1); // todo
        public Task<T?> GetAsync(int param1); // todo
    }
}
