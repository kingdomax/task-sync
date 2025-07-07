using System.Text.Json;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TaskCommentEntity>()
                .Property(e => e.MetaData)
                .HasColumnName("metadata")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, new JsonSerializerOptions())!
                )
                .Metadata.SetValueComparer(new ValueComparer<Dictionary<string, object>>(
                    (d1, d2) => JsonSerializer.Serialize(d1, new JsonSerializerOptions()) == JsonSerializer.Serialize(d2, new JsonSerializerOptions()),
                    d => JsonSerializer.Serialize(d, new JsonSerializerOptions()).GetHashCode(),
                    d => JsonSerializer.Deserialize<Dictionary<string, object>>(JsonSerializer.Serialize(d, new JsonSerializerOptions()), new JsonSerializerOptions())!
                ));
        }

        // public override async ValueTask DisposeAsync()
        // {
        //    Console.WriteLine("DbContext DisposeAsync");
        //    await base.DisposeAsync();
        // }
    }
}
