using Microsoft.AspNetCore.SignalR;

namespace StarWarsServer.Hubs;

public class GameHub : Hub
{
    public Task<string> SendTest(string message)
    {
        Console.WriteLine($"📡 Received from client: {message}");
        var reply = $"Pong from server: {message}";
        return Task.FromResult(reply);
    }


}
