using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string room, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, room, message);
        }
    }
}
