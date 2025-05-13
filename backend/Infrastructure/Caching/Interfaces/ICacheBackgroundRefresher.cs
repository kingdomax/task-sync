namespace TaskSync.Infrastructure.Caching.Interfaces
{
    public interface ICacheBackgroundRefresher
    {
        void RefreshProjectTasks(int projectId);
    }
}
