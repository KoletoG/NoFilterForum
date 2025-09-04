using Core.Enums;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Security.Claims;
using Ganss.Xss;
using Core.Models.DTOs.InputDTOs;
using NoFilterForum.Core.Models.ViewModels;
using Core.Constants;
using Core.Utility;
using Web.Mappers;
using Web.ViewModels.Post;
using System.Runtime.CompilerServices;
using Core.DTOs.OutputDTOs.Reply;
using Microsoft.AspNetCore.Identity;
using Core.DTOs.InputDTOs.Post;

namespace Web.Controllers
{
    public class PostController(IPostService postService, ISectionService sectionService, UserManager<UserDataModel> userManager) : Controller
    {
        private readonly IPostService _postService = postService;
        private readonly ISectionService _sectionService = sectionService;
        private readonly UserManager<UserDataModel> _userManager = userManager;
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostViewModel createVM, CancellationToken cancellationToken)
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
            if (await _postService.HasTimeoutAsync(userId, cancellationToken))
            {
                return RedirectToAction(nameof(Index), new { title = createVM.TitleOfSection, errorTime = true }); // Need to change errorTime
            }
            createVM.TitleOfSection = HttpUtility.UrlDecode(createVM.TitleOfSection);
            var createPostRequest = PostMapper.MapToRequest(createVM, userId);
            var result = await _postService.CreatePostAsync(createPostRequest, cancellationToken);
            return result switch
            {
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                PostResult.Success => RedirectToAction(nameof(Index), new { titleOfSection = HttpUtility.UrlEncode(createVM.TitleOfSection) }),
                _ => Problem("Invalid result")
            };
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Search( string text)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }
            var result = await _postService.SearchByText(text);
            return Ok(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Like(LikeDislikePostViewModel likeDislikePostViewModel, CancellationToken cancellationToken)
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
            var likeDislikeRequest = PostMapper.MapToRequest(likeDislikePostViewModel.Id, userId);
            var result = await _postService.LikeAsync(likeDislikeRequest, cancellationToken);
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
        public async Task<IActionResult> Dislike(LikeDislikePostViewModel likeDislikePostViewModel,CancellationToken cancellationToken)
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
            var likeDislikeRequest = PostMapper.MapToRequest(likeDislikePostViewModel.Id, userId);
            var result = await _postService.DislikeAsync(likeDislikeRequest,cancellationToken);
            return result switch
            {
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                PostResult.Success => NoContent(),
                _ => Problem()
            };
        }

        private async Task<PageTotalPagesDTO> GetPagesTotalPagesDtoAsync(string titleOfSection, int page)
        {
            var totalPostsCount = await _postService.GetPostsCountBySectionTitleAsync(titleOfSection);
            return PageUtility.GetPageTotalPagesDTO(page, totalPostsCount);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(string titleOfSection, CancellationToken cancellationToken, int page = 1)
        {
            titleOfSection = HttpUtility.UrlDecode(titleOfSection);
            if (!await _sectionService.ExistsSectionByTitleAsync(titleOfSection, cancellationToken))
            {
                return NotFound($"Section with title: {titleOfSection} doesn't exist");
            }
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return Unauthorized();
            }
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            var pageTotalPagesDTO = await GetPagesTotalPagesDtoAsync(titleOfSection, page);
            var getIndexPostRequest = PostMapper.MapToRequest(page, titleOfSection,user.Id);
            var postDtoList = await _postService.GetPostItemDtosByTitleAndPageAsync(getIndexPostRequest, cancellationToken);
            var postIndexItemsVMs = postDtoList.Select(PostMapper.MapToViewModel);
            
            var postIndexViewModel = PostMapper.MapToViewModel(postIndexItemsVMs, pageTotalPagesDTO, titleOfSection,isAdmin);

            return View(postIndexViewModel);
        }
        [HttpPost] 
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeletePostViewModel deletePostViewModel, CancellationToken cancellationToken)
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
            var sectionTitle = await _postService.GetSectionTitleByPostIdAsync(deletePostViewModel.PostId, cancellationToken);
            var deletePostRequest = PostMapper.MapToRequest(deletePostViewModel, userId);
            var result = await _postService.DeletePostByIdAsync(deletePostRequest, cancellationToken);
            if (result != PostResult.Success)
            {
                return result switch
                {
                    PostResult.NotFound => NotFound(),
                    PostResult.Forbid => Forbid(),
                    PostResult.UpdateFailed => Problem(),
                    _ => Problem()
                };
            }
            return RedirectToAction(nameof(Index), new { titleOfSection = HttpUtility.UrlEncode(sectionTitle) });
        }
    }
}
