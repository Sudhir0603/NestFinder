using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NestFinder.Hubs
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> ConnectedUsers = new Dictionary<string, string>();

        public void Connect(string username)
        {
            if (!ConnectedUsers.ContainsKey(Context.ConnectionId))
            {
                ConnectedUsers.Add(Context.ConnectionId, username);
            }
            Clients.All.updateUserList(ConnectedUsers.Values.ToList());
        }

        public void SendMessage(string sender, string receiver, string message)
        {
            Clients.All.receiveMessage(sender, receiver, message);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (ConnectedUsers.ContainsKey(Context.ConnectionId))
            {
                ConnectedUsers.Remove(Context.ConnectionId);
                Clients.All.updateUserList(ConnectedUsers.Values.ToList());
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}
