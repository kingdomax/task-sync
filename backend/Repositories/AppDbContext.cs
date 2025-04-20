using Microsoft.EntityFrameworkCore;
using TaskSync.Repositories.Entities;

namespace TaskSync.Repositories
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<TaskEntity> Tasks => Set<TaskEntity>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            Console.WriteLine("DbContext created");
        }
    }
}