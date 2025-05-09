﻿using TaskSync.Models.Dto;

namespace TaskSync.SignalR.Interfaces
{
    public interface ITaskNotificationService
    {
        Task NotifyTaskUpdateAsync(TaskDto dto, string? excludeConnectionId = null);
    }
}
