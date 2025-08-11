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
            var listChatIndexDtos = await _chatService.GetIndexChatDTOsAsync(userId,cancellationToken);
            var chatIndexViewModels = listChatIndexDtos.Select(ChatMapper.MapToViewModel).ToList();
            return View(chatIndexViewModels);
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
            if (chatDTO.UserId1 != userId) return Forbid();
            var chatVM = ChatMapper.MapToViewModel(chatDTO);
            return View(chatVM);
        }
    }
}
