namespace TaskSync
{
    public class GamificationApi
    {
        public async Task UpdateScore()
        {
            // probably pass enum
            using var httpCLient = new HttpClient();

            var httpMessage = new HttpRequestMessage(HttpMethod.Post, "");

            _ = httpCLient.SendAsync(httpMessage);
        }
    }
}
