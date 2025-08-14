using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.SignalRHubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string userId, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", userId, message);
        }
    }
}
