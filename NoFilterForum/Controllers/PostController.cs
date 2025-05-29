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
using Web.ViewModels;
using Core.Models.DTOs.InputDTOs;
using Web.Mappers.Posts;
using NoFilterForum.Core.Models.ViewModels;
using Core.Constants;
using Core.Utility;

namespace Web.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        public PostController(IPostService postService, IUserService userService)
        {
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
                return RedirectToAction("PostsMain", "Home", new { title = createVM.TitleOfSection, errorTime = true }); // Need to change errorTime
            }
            createVM.TitleOfSection = HttpUtility.UrlDecode(createVM.TitleOfSection);
            var createPostRequest = PostMappers.MapToRequest(createVM, userId);
            var result = await _postService.CreatePostAsync(createPostRequest);
            return result switch
            {
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                PostResult.Success => RedirectToAction("PostsMain","Home", new { title = HttpUtility.UrlEncode(createVM.TitleOfSection) }),
                _ => Problem("Invalid result")
            };
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(GetPostsViewModel postsViewModel)
        {
            postsViewModel.TitleOfSection = HttpUtility.UrlDecode(postsViewModel.TitleOfSection);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var currentUser = await _userService.GetUserByIdAsync(userId);
            if (currentUser == null)
            {
                return NotFound();
            }
            var totalPostsCount = await _postService.GetPostsCountBySectionTitleAsync(postsViewModel.TitleOfSection);
            var totalPages = PageUtility.GetTotalPagesCount(totalPostsCount, PostConstants.PostsPerSection);
            postsViewModel.Page = PageUtility.ValidatePageNumber(postsViewModel.Page, totalPages);
            var getIndexPostRequest = PostMappers.MapToRequest(postsViewModel);
            var postDtoList = await _postService.GetPostItemDtosByTitleAndPageAsync(getIndexPostRequest);
            postsViewModel.TitleOfSection = HttpUtility.UrlEncode(postsViewModel.TitleOfSection);
            var postIndexItemsVMs = postDtoList.Select(PostMappers.MapToViewModel).ToList();
            var postIndexViewModel = PostMappers.MapToViewModel(postIndexItemsVMs, postsViewModel.Page, totalPages, postsViewModel.TitleOfSection);
            return View(postIndexViewModel);
        }
    }
}
