using Microsoft.EntityFrameworkCore;

using TaskSync.Repositories.Entities;

namespace TaskSync.Repositories
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
        public DbSet<TaskEntity> Tasks => Set<TaskEntity>();
        public DbSet<TaskCommentEntity> TaskComments => Set<TaskCommentEntity>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            // Console.WriteLine("DbContext created");
        }

        // public override async ValueTask DisposeAsync()
        // {
        //    Console.WriteLine("DbContext DisposeAsync");
        //    await base.DisposeAsync();
        // }
    }
}
