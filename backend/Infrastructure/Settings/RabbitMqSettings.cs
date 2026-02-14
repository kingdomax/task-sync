namespace TaskSync.Infrastructure.Settings
{
    public class RabbitMqSettings
    {
        public string Host { get; set; } = "localhost";
        public string Username { get; set; } = "user";
        public string Password { get; set; } = "password";
    }
}
