using Moq;
using TaskSync.ExternalApi.Interfaces;
using TaskSync.Infrastructure.Caching.Interfaces;
using TaskSync.Infrastructure.Http.Interface;
using TaskSync.Repositories.Entities;
using TaskSync.Repositories.Interfaces;
using TaskSync.Services;
using TaskSync.Services.Interfaces;
using TaskSync.SignalR.Interfaces;


namespace TaskSyncTest.Services
{
    // Test method run parallel in the same file as well as different test class
    public class TaskServiceTest
    {
        private TaskService CreateTaskService(
            TaskEntity? returnEntity,
            out Mock<ITaskNotificationService> notificationMock,
            out Mock<IMemoryCacheService<IList<TaskEntity>>> taskCacheMock,
            out Mock<ICacheBackgroundRefresher> cacheBgRefresherMock,
            out Mock<IGamificationApi> gamificationApiMock)
        {
            var http = new Mock<IHttpContextReader>();
            var repo = new Mock<ITaskRepository>();
            var taskHub = new Mock<ITaskNotificationService>();
            var taskEntityCache = new Mock<IMemoryCacheService<IList<TaskEntity>>>();
            var cacheBgRefresher = new Mock<ICacheBackgroundRefresher>();
            var gamificationApi = new Mock<IGamificationApi>();
            var commentService = new Mock<ICommentService>();
            var projectRepo = new Mock<IProjectRepository>();
            var projectEntityCache = new Mock<IMemoryCacheService<ProjectEntity>>();

            repo.Setup(x => x.UpdateStatusAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(returnEntity);
            taskCacheMock = taskEntityCache;
            notificationMock = taskHub;
            cacheBgRefresherMock = cacheBgRefresher;
            gamificationApiMock = gamificationApi;

            return new TaskService(http.Object, repo.Object, taskCacheMock.Object, taskHub.Object, cacheBgRefresher.Object, gamificationApi.Object, commentService.Object, projectRepo.Object, projectEntityCache.Object);
        }

        [Fact]
        public async Task UpdateTaskStatusAsync_ShouldReturnNull_WhenUpdateFails()
        {
            var service = CreateTaskService(null, out var notificationMock, out var taskCacheMock, out var cacheBgRefresherMock, out var gamificationApiMock);

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
            var service = CreateTaskService(mockTaskEntity, out var notificationMock, out var taskCacheMock, out var cacheBgRefresherMock, out var gamificationApiMock);

            var result = await service.UpdateTaskStatusAsync(2, new UpdateTaskRequest() { StatusRaw = "TODO" });

            Assert.Equal(mockTaskEntity.Id, result?.Id);
            Assert.Equal(mockTaskEntity.Title, result?.Title);
            Assert.Equal(mockTaskEntity.AssigneeId, result?.AssigneeId);
            Assert.Equal(mockTaskEntity.Status, result?.Status);
            Assert.Equal(mockTaskEntity.LastModified, result?.LastModified);
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
            var service = CreateTaskService(mockTaskEntity, out var notificationMock, out var taskCacheMock, out var cacheBgRefresherMock, out var gamificationApiMock);

            var result = await service.UpdateTaskStatusAsync(4, new UpdateTaskRequest() { StatusRaw = "INPROGRESS" });

            if (result != null)
            {
                notificationMock.Verify(x => x.NotifyTaskUpdateAsync(result, It.IsAny<string?>()), Times.Once);
            }
            else
            {
                Assert.Fail("Result should not be null when task update is successful.");
            }
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
            var service = CreateTaskService(mockTaskEntity, out var notificationMock, out var taskCacheMock, out var cacheBgRefresherMock, out var gamificationApiMock);

            var result = await service.UpdateTaskStatusAsync(5, new UpdateTaskRequest() { StatusRaw = "DONE" });

            cacheBgRefresherMock.Verify(x => x.RefreshProjectTasks(mockTaskEntity.ProjectId), Times.Once);
        }

        [Fact]
        public async Task UpdateTaskStatusAsync_ShouldCallGamificationApi_WhenTaskUpdated()
        {
            var mockTaskEntity = new TaskEntity()
            {
                Id = 6,
                Title = "Comment support on task",
                AssigneeId = 6,
                StatusRaw = "DONE",
                LastModified = new DateTime(2025, 6, 6),
                ProjectId = 6,
            };
            var service = CreateTaskService(mockTaskEntity, out var notificationMock, out var taskCacheMock, out var cacheBgRefresherMock, out var gamificationApiMock);

            var result = await service.UpdateTaskStatusAsync(6, new UpdateTaskRequest() { StatusRaw = "DONE" });

            if (result != null)
            {
                gamificationApiMock.Verify(x => x.UpdatePoint(result.Id, result.Status), Times.Once);
            }
            else
            {
                Assert.Fail("Result should not be null when task update is successful.");
            }
        }
    }
}
