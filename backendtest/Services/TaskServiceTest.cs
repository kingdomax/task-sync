﻿using Moq;
using TaskSync.Infrastructure.Caching.Interfaces;
using TaskSync.Infrastructure.Http.Interface;
using TaskSync.Models.Dto;
using TaskSync.Repositories.Entities;
using TaskSync.Repositories.Interfaces;
using TaskSync.Services;
using TaskSync.SignalR.Interfaces;


namespace TaskSyncTest.Services
{
    // Test method run parallel in the same file as well as different test class
    public class TaskServiceTest
    {
        private TaskService CreateTaskService(
            TaskEntity? returnEntity,
            out Mock<ITaskNotificationService> notificationMock,
            out Mock<IMemoryCacheService<IList<TaskEntity>>> cacheMock,
            out Mock<ICacheBackgroundRefresher> cacheBgRefresherMock)
        {
            var http = new Mock<IHttpContextReader>();
            var repo = new Mock<ITaskRepository>();
            var taskHub = new Mock<ITaskNotificationService>();
            var cache = new Mock<IMemoryCacheService<IList<TaskEntity>>>();
            var cacheBgRefresher = new Mock<ICacheBackgroundRefresher>();

            repo.Setup(x => x.UpdateStatusAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(returnEntity);
            cacheMock = cache;
            notificationMock = taskHub;
            cacheBgRefresherMock = cacheBgRefresher;

            return new TaskService(http.Object, repo.Object, cache.Object, taskHub.Object, cacheBgRefresher.Object);
        }

        [Fact]
        public async Task UpdateTaskStatusAsync_ShouldReturnNull_WhenUpdateFails()
        {
            var service = CreateTaskService(null, out var notificationMock, out var cacheMock, out var cacheBgRefresherMock);

            var result = await service.UpdateTaskStatusAsync(1, new UpdateTaskRequest() { StatusRaw = "BACKLOG" });

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateTaskStatusAsync_ShouldReturnDto_WhenUpdateSucceeds()
        {
            var mockTaskEntity = new TaskEntity()
            {
                Id = 2,
                Title = "Setup CI/CD",
                AssigneeId = 2,
                StatusRaw = "TODO",
                LastModified = new DateTime(2025, 2, 2)
            };
            var service = CreateTaskService(mockTaskEntity, out var notificationMock, out var cacheMock, out var cacheBgRefresherMock);

            var result = await service.UpdateTaskStatusAsync(2, new UpdateTaskRequest() { StatusRaw = "TODO" });

            Assert.Equal(mockTaskEntity.Id, result?.Id);
            Assert.Equal(mockTaskEntity.Title, result?.Title);
            Assert.Equal(mockTaskEntity.AssigneeId, result?.AssigneeId);
            Assert.Equal(mockTaskEntity.Status, result?.Status);
            Assert.Equal(mockTaskEntity.LastModified, result?.LastModified);
        }

        [Fact]
        public async Task UpdateTaskStatusAsync_ShouldEvictCache_WhenTaskUpdated()
        {
            var mockTaskEntity = new TaskEntity()
            {
                Id = 3,
                Title = "Integrate Websocket flow",
                AssigneeId = 3,
                StatusRaw = "INPROGRESS",
                LastModified = new DateTime(2025, 3, 3),
                ProjectId = 3, // make sure it evict correct cache key
            };
            var service = CreateTaskService(mockTaskEntity, out var notificationMock, out var cacheMock, out var cacheBgRefresherMock);

            var result = await service.UpdateTaskStatusAsync(3, new UpdateTaskRequest() { StatusRaw = "INPROGRESS" });

            cacheMock.Verify(x => x.Remove(mockTaskEntity.ProjectId), Times.Once);
        }

        [Fact]
        public async Task UpdateTaskStatusAsync_ShouldNotifyOtherClients_WhenTaskUpdated()
        {
            var mockTaskEntity = new TaskEntity()
            {
                Id = 4,
                Title = "Add timming middleware",
                AssigneeId = 4,
                StatusRaw = "DONE",
                LastModified = new DateTime(2025, 4, 4),
            };
            var service = CreateTaskService(mockTaskEntity, out var notificationMock, out var cacheMock, out var cacheBgRefresherMock);

            var result = await service.UpdateTaskStatusAsync(4, new UpdateTaskRequest() { StatusRaw = "INPROGRESS" });

            notificationMock.Verify(x => x.NotifyTaskUpdateAsync(It.IsAny<TaskDto>(), It.IsAny<string?>()), Times.Once);
        }

        [Fact]
        public async Task UpdateTaskStatusAsync_ShouldPreWarmCacheInBackground_WhenTaskUpdated()
        {
            var mockTaskEntity = new TaskEntity()
            {
                Id = 5,
                Title = "Award points for task completion and other contributions",
                AssigneeId = 5,
                StatusRaw = "DONE",
                LastModified = new DateTime(2025, 5, 5),
                ProjectId = 5,
            };
            var service = CreateTaskService(mockTaskEntity, out var notificationMock, out var cacheMock, out var cacheBgRefresherMock);

            var result = await service.UpdateTaskStatusAsync(5, new UpdateTaskRequest() { StatusRaw = "DONE" });

            cacheBgRefresherMock.Verify(x => x.RefreshProjectTasks(mockTaskEntity.ProjectId), Times.Once);
        }
    }
}
