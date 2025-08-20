using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Core.DTOs.InputDTOs.Chat;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Web.Mappers;
using Web.ViewModels.Chat;
using Web.ViewModels.Message;

namespace Web.Controllers
{
    public class MessageController(IMessageService messageService) : Controller
    {
        private readonly IMessageService _messageService = messageService;
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(CreateMessageViewModel createMessageViewModel, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId is null) return Unauthorized();
            var request = MessageMapper.MapToRequest(createMessageViewModel, userId);
            var result = await _messageService.CreateMessageAsync(request, cancellationToken);
            return result.Result switch
            {
                PostResult.Success => Ok(new { result.Message, result.MessageId }),
                PostResult.Forbid => Forbid(),
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateLastMessage(UpdateLastMessageViewModel viewModel, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId is null) return Unauthorized();

            return NoContent();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(DeleteMessageViewModel deleteMessageViewModel,CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId is null) return Unauthorized();
            var result = await _messageService.DeleteAsync(deleteMessageViewModel.MessageId, userId);
            return result switch
            {
                PostResult.Success => NoContent(),
                PostResult.Forbid => Forbid(),
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
        }
    }
}
