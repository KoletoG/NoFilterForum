using System.Security.Claims;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Mappers;
using Web.ViewModels.Chat;

namespace Web.Controllers
{
    public class ChatController(IChatService chatService) : Controller
    {
        private readonly IChatService _chatService = chatService;
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }
            var username = User.Identity!.Name!;
            var listChatIndexDtos = await _chatService.GetIndexChatDTOsAsync(userId, username,cancellationToken);
            var chatIndexViewModels = listChatIndexDtos.Select(ChatMapper.MapToViewModel).ToList();
            return View(chatIndexViewModels);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetLastMessage([FromQuery] string chatId,CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Unauthorized();
            var result = await _chatService.GetMessageIdOfLastMessageAsync(userId,chatId,cancellationToken);
            return Ok(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> UpdateLastMessage([FromBody] UpdateLastMessageViewModel viewModel, CancellationToken cancellationToken)
        {
            var request = ChatMapper.MapToRequest(viewModel, viewModel.UserId);
            var result = await _chatService.UpdateLastMessageAsync(request, cancellationToken);
            return result switch
            {
                PostResult.Success => NoContent(),
                PostResult.NotFound => NotFound(),
                PostResult.Forbid => Forbid(),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateChatViewModel createChatViewModel, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }
            var result = await _chatService.CreateChat(userId, createChatViewModel.UserId, cancellationToken);
            return result switch
            {
                PostResult.Success => RedirectToAction("Index"),
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                PostResult.Conflict => Conflict(),
                _ => Problem()
            };
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(string id,CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Unauthorized();
            var chatDTO = await _chatService.GetChat(id,userId,cancellationToken);
            if (chatDTO is null) return BadRequest();
            if (chatDTO.UserId1 != userId && chatDTO.UserId2 !=userId) return Forbid();
            var chatVM = ChatMapper.MapToViewModel(chatDTO,userId);
            return View(chatVM);
        }
    }
}
