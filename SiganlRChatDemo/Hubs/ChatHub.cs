using Microsoft.AspNetCore.SignalR;

namespace SiganlRChatDemo.Hubs
{
    public class ChatHub:Hub
    {
        private static Dictionary<string, string> connectedClients = new Dictionary<string, string>();
        public async Task SendMessage(string username,string message) 
        {
            await Clients.All.SendAsync("ReceiveMessage", username, message);            
        }
        public async Task JoinChat(string user, string message)
        {
            connectedClients[Context.ConnectionId] = user;
            await Clients.Others.SendAsync("ReceiveMessage", user, message);
        }

        private async Task LeaveChat()
        {
            if (connectedClients.TryGetValue(Context.ConnectionId, out string user))    //Will put First arg value(Not Key(coz key is 1st arg)) to 2nd arg-- Here if returs true if key is found n vice versa
            {
                var message = $"{user} left the chat";
                await Clients.Others.SendAsync("ReceiveMessage", user, message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await LeaveChat();
            await base.OnDisconnectedAsync(exception);

        }
    }
}
