namespace TaskSync.Repositories
{
    public class TasksRepository
    {
        private readonly AppDbContext _dbContext;
        public TasksRepository(AppDbContext dbContext) => _dbContext = dbContext;
    }
}
