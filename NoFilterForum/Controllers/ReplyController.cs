using Application.Interfaces.Services;
using Core.DTOs.OutputDTOs.Reply;
using Core.Enums;
using Core.Models.DTOs.OutputDTOs;
using Core.Models.DTOs.OutputDTOs.Reply;
using Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NoFilterForum.Core.Interfaces.Services;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Web.Mappers;
using Web.ViewModels.Post;
using Web.ViewModels.Reply;

namespace Web.Controllers
{
    public class ReplyController(IReplyService replyService, IPostService postService, IUserService userService) : Controller
    {
        private readonly IReplyService _replyService = replyService;
        private readonly IPostService _postService = postService;
        private readonly IUserService _userService = userService;

        private async Task<PageTotalPagesDTO> GetPageInfoAsync(string postId, string? replyId, int page)
        {
            return string.IsNullOrEmpty(replyId)
                ? await _replyService.GetPageAndTotalPagesDTOByPostIdAsync(postId, page)
                : await _replyService.GetPageTotalPagesDTOByReplyIdAndPostIdAsync(replyId, postId);
        }
        private void MarkTags(IEnumerable<ReplyIndexItemViewModel> replyItemsVM, PostReplyIndexViewModel postReplyIndexViewModel)
        {
            string currentUsername = User.Identity!.Name!; // We have [Authorize] so this can't be null
            foreach (var replyItemVM in replyItemsVM)
            { 
                replyItemVM.MarkTags(currentUsername);            
            }
            postReplyIndexViewModel.MarkTags(currentUsername);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(string postId,CancellationToken cancellationToken, string? replyId = null, int page = 1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }

            var currentUserDto = await _userService.GetCurrentUserReplyIndexDtoByIdAsync(userId, cancellationToken);
            if (currentUserDto is null)
            {
                return Unauthorized();
            }

            var post = await _postService.GetPostReplyIndexDtoByIdAsync(postId, cancellationToken);
            if (post is null)
            {
                return BadRequest();
            }

            var pageTotalPagesDto = await GetPageInfoAsync(postId, replyId, page);

            var getListReplyIndexItemRequest = ReplyMapper.MapToRequest(pageTotalPagesDto.Page, postId);
            var listReplyIndexDto = await _replyService.GetListReplyIndexItemDto(getListReplyIndexItemRequest, cancellationToken);

            var replyIndexVMList = listReplyIndexDto.Select(ReplyMapper.MapToViewModel).ToList();
            var postVM = ReplyMapper.MapToViewModel(post);

            MarkTags(replyIndexVMList, postVM);

            var currentUserVM = ReplyMapper.MapToViewModel(currentUserDto);
            var indexReplyVM = ReplyMapper.MapToViewModel(currentUserVM,
                postVM,
                replyIndexVMList,
                pageTotalPagesDto,
                replyId ?? string.Empty);
            return View(indexReplyVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Like(LikeDislikeReplyViewModel likeDislikeReplyViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }
            var likeDislikeRequest = PostMapper.MapToRequest(likeDislikeReplyViewModel.Id, userId);
            var result = await _replyService.LikeAsync(likeDislikeRequest);
     
            return result switch
            {
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                PostResult.Success => NoContent(),
                _ => Problem()
            };
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Dislike(LikeDislikeReplyViewModel likeDislikeReplyViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }
            var likeDislikeRequest = PostMapper.MapToRequest(likeDislikeReplyViewModel.Id, userId);
            var result = await _replyService.DislikeAsync(likeDislikeRequest);
            return result switch
            {
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                PostResult.Success => NoContent(),
                _ => Problem()
            };
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(DeleteReplyViewModel deleteReplyViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
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
                PostResult.Success => RedirectToAction(nameof(Index), new { postId = deleteReplyViewModel.PostId }),
                _ => Problem()
            };
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(CreateReplyViewModel createReplyViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }
            if (await _replyService.HasTimeoutByUserIdAsync(userId))
            {
                ModelState.AddModelError("timeError", "Replies can be made every 5 seconds");
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            var createReplyRequest = ReplyMapper.MapToRequest(createReplyViewModel, userId);
            var result = await _replyService.CreateReplyAsync(createReplyRequest);
            return result switch
            {
                PostResult.UpdateFailed => Problem(),
                PostResult.Forbid => Forbid(),
                PostResult.NotFound => NotFound(),
                PostResult.Success => RedirectToAction(nameof(Index), new { postId = createReplyViewModel.PostId }),
                _ => Problem()
            };
        }
    }
}
