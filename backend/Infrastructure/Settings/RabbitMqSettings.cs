namespace TaskSync.Infrastructure.Settings
{
    public class RabbitMqSettings
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string Username { get; set; } = "user";
        public string Password { get; set; } = "password";
        public string VirtualHost { get; set; } = "/";
    }
}
