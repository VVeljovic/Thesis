
using Microsoft.AspNetCore.SignalR;

public class NotificationService : Hub<INotificationService>
{
    public override async Task OnConnectedAsync()
    {
        await Clients.Client(Context.ConnectionId).ReceiveNotification("Connected");
        await base.OnConnectedAsync();
    }
}