using TaskSync.Infrastructure.Http.Interface;

namespace TaskSync
{
    public class PointDto // todo-moch: move to new file and create folder
    {
        public int TaskId { get; set; }
        public int ActionId { get; set; } // todo-moch: this sohould probably be enum for consistency betwween all api
        public string? UserId { get; set; }
    }

    public interface IGamificationApi // todo-moch: move to new file and create folder
    {
        Task UpdatePoint(int taskId, int actionId);
    }

    public class GamificationApi : IGamificationApi
    {
        private readonly IHttpContextReader _httpContextReader;

        public GamificationApi(IHttpContextReader httpContextReader) // todo-moch: should it be singleton or not
        {
            _httpContextReader = httpContextReader;
        }

        public async Task UpdatePoint(int taskId, int actionId)
        {
            try
            {
                using var httpClient = new HttpClient();

                var httpMessage = new HttpRequestMessage(HttpMethod.Post, "localhost:1234/api/point") // todo-moch: put in config file
                {
                    Content = JsonContent.Create(new PointDto()
                    {
                        TaskId = taskId,
                        ActionId = actionId,
                        UserId = _httpContextReader.GetUserId(),
                    }),
                };

                await httpClient.SendAsync(httpMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
