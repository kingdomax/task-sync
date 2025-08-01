using TaskSync.ExternalApi.Interfaces;
using TaskSync.Infrastructure.Http.Interface;
using TaskSync.Models.Dto;

using TaskStatus = TaskSync.Enums.TASK_STATUS;

namespace TaskSync.ExternalApi
{
    public class GamificationApi : IGamificationApi
    {
        private readonly IHttpContextReader _httpContextReader;
        private readonly HttpClient _httpClient;
        private readonly ILogger<GamificationApi> _logger;

        public GamificationApi(IHttpContextReader httpContextReader, IHttpClientFactory httpClientFactory, ILogger<GamificationApi> logger)
        {
            _httpContextReader = httpContextReader;
            _httpClient = httpClientFactory.CreateClient("GamificationApi");
            _logger = logger;
        }

        public async Task UpdatePoint(int taskId, TaskStatus status)
        {
            try
            {
                var httpMessage = new HttpRequestMessage(HttpMethod.Post, "points")
                {
                    Content = JsonContent.Create(new CreatePointDto()
                    {
                        TaskId = taskId,
                        TaskStatus = status,
                        UserId = _httpContextReader.GetUserId(),
                    }),
                };

                await _httpClient.SendAsync(httpMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
