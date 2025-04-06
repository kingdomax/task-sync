using TaskSync.Repositories.Entity;
using Microsoft.EntityFrameworkCore;

namespace TaskSync.Repositories
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserEntity> Users => Set<UserEntity>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            Console.WriteLine("DbContext created");
        }

        //public override async ValueTask DisposeAsync()
        //{
        //    Console.WriteLine("DbContext disposed (async)");
        //    await base.DisposeAsync();
        //}
    }
}