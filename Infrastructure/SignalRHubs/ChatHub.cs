using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.SignalRHubs
{
    public class ChatHub(IChatService chatService) : Hub
    {
        private readonly IChatService _chatService = chatService;
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task SendMessage(string userId, string message,string messageId)
        {
            if (await _chatService.ExistChatByUserIdsAsync(userId, Context.User!.FindFirstValue(ClaimTypes.NameIdentifier)!, CancellationToken.None))
            {
                await Clients.User(userId).SendAsync("ReceiveMessage", message,messageId);
            }
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task DeleteMessage(string userId, string messageId)
        {
            await Clients.User(userId).SendAsync("RemoveMessage", messageId);
        }
    }
}
