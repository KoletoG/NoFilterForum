using Core.Enums;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Core.Models.GetViewModels;
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

namespace Web.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly ISectionService _sectionService;
        public PostController(IPostService postService, ISectionService sectionService, IUserService userService)
        {
            _sectionService = sectionService;
            _userService = userService;
            _postService = postService;
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostViewModel createVM)
        {
            if (!ModelState.IsValid)
            {
                string errorsJson = JsonSerializer.Serialize(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                errorsJson = HttpUtility.HtmlEncode(errorsJson);
                return RedirectToAction("Index", "Home", new { errors = errorsJson }); // Change that
            }
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            if (await _postService.HasTimeoutAsync(userId))
            {
                return RedirectToAction("Index", "Post", new { title = createVM.TitleOfSection, errorTime = true }); // Need to change errorTime
            }
            createVM.TitleOfSection = HttpUtility.UrlDecode(createVM.TitleOfSection);
            var createPostRequest = PostMappers.MapToRequest(createVM, userId);
            var result = await _postService.CreatePostAsync(createPostRequest);
            return result switch
            {
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                PostResult.Success => RedirectToAction("Index", "Post", new { titleOfSection = HttpUtility.UrlEncode(createVM.TitleOfSection) }),
                _ => Problem("Invalid result")
            };
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Like(LikeDislikePostViewModel likeDislikePostViewModel)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var likeDislikeRequest = PostMappers.MapToRequest(likeDislikePostViewModel.Id, userId);
            var result = await _postService.LikeAsync(likeDislikeRequest);
            return result switch
            {
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                PostResult.Success => NoContent()
            };
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Dislike(LikeDislikePostViewModel likeDislikePostViewModel)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var likeDislikeRequest = PostMappers.MapToRequest(likeDislikePostViewModel.Id, userId);
            var result = await _postService.DislikeAsync(likeDislikeRequest);
            return result switch
            {
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                PostResult.Success => NoContent()
            };
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(string titleOfSection, int page = 1)
        {
            // Error TIME FIX
            titleOfSection = HttpUtility.UrlDecode(titleOfSection);
            bool sectionExists = await _sectionService.ExistsSectionByTitleAsync(titleOfSection);
            if (!sectionExists)
            {
                return NotFound($"Section with title: {titleOfSection} doesn't exist");
            }
            var totalPostsCount = await _postService.GetPostsCountBySectionTitleAsync(titleOfSection);
            var totalPages = PageUtility.GetTotalPagesCount(totalPostsCount, PostConstants.PostsPerSection);
            page = PageUtility.ValidatePageNumber(page, totalPages);
            var getIndexPostRequest = PostMappers.MapToRequest(page, titleOfSection);
            var postDtoList = await _postService.GetPostItemDtosByTitleAndPageAsync(getIndexPostRequest);
            var postIndexItemsVMs = postDtoList.Select(PostMappers.MapToViewModel).ToList();
            var postIndexViewModel = PostMappers.MapToViewModel(postIndexItemsVMs, page, totalPages, titleOfSection);
            return View(postIndexViewModel);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeletePostViewModel deletePostViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(); // Change this
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var deletePostRequest = PostMappers.MapToRequest(deletePostViewModel, userId);
            var result = await _postService.DeletePostByIdAsync(deletePostRequest);
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
            var sectionTitle = await _postService.GetSectionTitleByPostIdAsync(deletePostViewModel.PostId);
            return RedirectToAction("Index", new { titleOfSection = sectionTitle });

        }
    }
}
