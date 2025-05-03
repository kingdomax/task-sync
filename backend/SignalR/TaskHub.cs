using Microsoft.AspNetCore.SignalR;

namespace TaskSync.SignalR
{
    public class TaskHub : Hub
    {
        // Will be called from clientside
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"[SignalR] Client connected: {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"[SignalR] Client disconnected: {Context.ConnectionId}");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
