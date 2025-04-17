namespace TaskSync.Repositories
{
    public class ProjectsRepository
    {
        private readonly AppDbContext _dbContext;
        public ProjectsRepository(AppDbContext dbContext) => _dbContext = dbContext;
    }
}
