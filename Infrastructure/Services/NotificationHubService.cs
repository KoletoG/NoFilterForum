using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.Hub;
using Infrastructure.SignalRHubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Services
{
    public class NotificationHubService : INotificationHub
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationHubService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        [Authorize] 
        public async Task SendNotificationAsync(IEnumerable<string> userRecIds)
        {
            await _hubContext.Clients.Users(userRecIds).SendAsync("ReceiveNotification");
        }
    }
}
