namespace Learning.Signals.Hubs;

using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    readonly ILogger<ChatHub> _logger;

    public ChatHub(ILogger<ChatHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        await base.Clients.All.SendCoreAsync("Connect", new[] { "hi" });
    }

    // the signalR client can call "SendMessage"
    // the client can listen to "ReceiveMessage"
    public async Task SendMessage(SendMessage msg)
    {
        _logger.LogInformation("User: {User} Message: {Message} ({Screen})", msg.User, msg.Message, msg.ScreenId);

        await Clients.Group(msg.ScreenId).SendAsync("ReceiveMessage", msg.User, msg.Message);
    }

    public async Task Navigate(NavigateMessage msg)
    {
        _logger.LogInformation("Navigate to: {Path} ({Screen})", msg.Url, msg.Screen);

        // TODO: targets a specific screen
        await Clients.Group(msg.Screen).SendAsync("Navigate", msg.Url);
    }

    public async Task JoinScreen(string screen)
    {
        _logger.LogInformation("JOIN GROUP {Screen}", screen);
        await Groups.AddToGroupAsync(Context.ConnectionId, screen);
    }

    public async Task LeaveScreen(string screen)
    {
        _logger.LogInformation("LEAVE GROUP {Screen}", screen);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, screen);
    }

    // NextPage, PrevPage
    // SelectProduct
    // SelectVariant

}

public class SendMessage
{
    public string ScreenId { get; set; }
    public string User { get; set; }
    public string Message { get; set; }
}

public class NavigateMessage
{
    public string Screen { get; set; }
    public string Url { get; set; }
}
