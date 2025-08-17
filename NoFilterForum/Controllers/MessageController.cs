using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Web.Mappers;
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
                PostResult.Success => Ok(result.Message),
                PostResult.Forbid => Forbid(),
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(DeleteMessageViewModel deleteMessageViewModel,CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId is null) return Unauthorized();

            var result = await _messageService.DeleteAsync(deleteMessageViewModel.Id, userId);
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
