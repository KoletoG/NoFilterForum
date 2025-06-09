using Core.Enums;
using Core.Models.DTOs.OutputDTOs;
using Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Web.Mappers;
using Web.ViewModels.Reply;

namespace Web.Controllers
{
    public class ReplyController : Controller
    {
        private readonly IReplyService _replyService;
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        public ReplyController(IReplyService replyService, IPostService postService, IUserService userService)
        {
            _replyService = replyService;
            _postService = postService;
            _userService = userService;
        }
        public async Task<IActionResult> Index(string postId, string titleOfSection, string replyId = "", int page = 1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            if (!string.IsNullOrEmpty(replyId))
            {
                replyId = HttpUtility.UrlDecode(replyId);
            }
            postId = HttpUtility.UrlDecode(postId);
            var post = await _postService.GetPostReplyIndexDtoByIdAsync(postId);
            (page,var totalPages) = await _replyService.GetPageAndTotalPage(page, postId);
            var getListReplyIndexItemRequest = ReplyMapper.MapToRequest(page, postId);
            var listReplyIndexDto = await _replyService.GetListReplyIndexItemDto(getListReplyIndexItemRequest);
            var currentUsername = User.Identity.Name;
            _replyService.MarkTagsOfContents(ref listReplyIndexDto,ref post, currentUsername);
            var replyIndexVMList = listReplyIndexDto.Select(ReplyMapper.MapToViewModel).ToList();
            var postVM = ReplyMapper.MapToViewModel(post);
            var currentUserDto = await _userService.GetCurrentUserReplyIndexDtoByIdAsync(userId);
            var currentUserVM = ReplyMapper.MapToViewModel(currentUserDto);
            var indexReplyVM = ReplyMapper.MapToViewModel(currentUserVM, 
                postVM,
                replyIndexVMList,
                page, 
                totalPages, 
                titleOfSection,
                replyId);
            return View(indexReplyVM); // Need to add viewmodels 
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
                PostResult.Success => RedirectToAction("PostView", "Home", new { id = deleteReplyViewModel.PostId, titleOfSection = deleteReplyViewModel.TitleOfSection }),
                _ => Problem()
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
                PostResult.Success => RedirectToAction("PostView", "Home", new { id = createReplyViewModel.PostId, titleOfSection = createReplyViewModel.Title }),
                _ => Problem()
                // Change the line above when PostView is refactored
            };
        }
    }
}
