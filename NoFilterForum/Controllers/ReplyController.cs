using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;
using System.Security.Claims;
using Web.Mappers.Reply;
using Web.ViewModels.Reply;

namespace Web.Controllers
{
    public class ReplyController : Controller
    {
        private readonly IReplyService _replyService;
        public ReplyController(IReplyService replyService)
        {
            _replyService = replyService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(DeleteReplyViewModel deleteReplyViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var deleteReplyRequest = ReplyMapper.MapToRequest(deleteReplyViewModel, userId);
            var result = await _replyService.DeleteReplyAsync(deleteReplyRequest);
            return result switch
            {
                PostResult.UpdateFailed => Problem(),
                PostResult.Forbid => Forbid(),
                PostResult.NotFound => NotFound($"Reply with Id: {deleteReplyViewModel.ReplyId} was not found"),
                PostResult.Success => RedirectToAction("PostView", "Home", new { id = deleteReplyViewModel.PostId, titleOfSection = deleteReplyViewModel.TitleOfSection })
                // Change previous line when updating PostView
            };
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(CreateReplyViewModel createReplyViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState); // Change when postview is done
            }
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            if (await _replyService.HasTimeoutByUserIdAsync(userId))
            {
                ModelState.AddModelError("timeError", "Replies can be made every 5 seconds");
                return Forbid(); // Change to show ModelState error
            }
            var createReplyRequest = ReplyMapper.MapToRequest(createReplyViewModel, userId);
            var result = await _replyService.CreateReplyAsync(createReplyRequest);
            return result switch
            {
                PostResult.UpdateFailed => Problem(),
                PostResult.Forbid => Forbid(),
                PostResult.NotFound => NotFound(),
                PostResult.Success => RedirectToAction("PostView", "Home", new { id = createReplyViewModel.PostId, titleOfSection = createReplyViewModel.Title })
                // Change the line above when PostView is refactored
            };
        }
    }
}
