﻿using Microsoft.Extensions.Options;

using TaskSync.ExternalApi.Interfaces;
using TaskSync.Infrastructure.Http.Interface;
using TaskSync.Infrastructure.Settings;
using TaskSync.Models.Dto;

using TaskStatus = TaskSync.Enums.TASK_STATUS;

namespace TaskSync.ExternalApi
{
    public class GamificationApi : IGamificationApi
    {
        private readonly GamificationApiSettings _gamApiSettings;
        private readonly IHttpContextReader _httpContextReader;
        private readonly HttpClient _httpClient;
        private readonly ILogger<GamificationApi> _logger;

        public GamificationApi(IOptions<GamificationApiSettings> options, IHttpContextReader httpContextReader, IHttpClientFactory httpClientFactory, ILogger<GamificationApi> logger)
        {
            _gamApiSettings = options.Value;
            _httpContextReader = httpContextReader;
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
        }

        public async Task UpdatePoint(int taskId, TaskStatus status)
        {
            try
            {
                var httpMessage = new HttpRequestMessage(HttpMethod.Post, $"{_gamApiSettings.BaseUrl}/points")
                {
                    Content = JsonContent.Create(new CreatePointDto()
                    {
                        TaskId = taskId,
                        TaskStatus = status,
                        UserId = _httpContextReader.GetUserId(),
                    }),
                };
                httpMessage.Headers.Add("x-gamapi-auth", "true");

                await _httpClient.SendAsync(httpMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
