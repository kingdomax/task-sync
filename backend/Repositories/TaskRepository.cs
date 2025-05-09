﻿using Microsoft.EntityFrameworkCore;

using TaskSync.Repositories.Entities;
using TaskSync.Repositories.Interfaces;

namespace TaskSync.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _dbContext;
        public TaskRepository(AppDbContext dbContext) => _dbContext = dbContext;

        public async Task<IList<TaskEntity>?> GetAsync(int projectId)
        {
            return await _dbContext.Tasks.Where(x => x.ProjectId == projectId).ToListAsync();
        }

        public async Task<TaskEntity?> UpdateStatusAsync(int taskId, string newStatus)
        {
            var task = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);
            if (task == null)
            {
                return null;
            }

            task.StatusRaw = newStatus.ToLower();
            task.LastModified = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return task;
        }
    }
}
