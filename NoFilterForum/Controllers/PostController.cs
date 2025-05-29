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
        private readonly ISectionService _sectionService;
        public PostController(IPostService postService,ISectionService sectionService, IUserService userService)
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
                return RedirectToAction("PostsMain", "Home", new { title = createVM.TitleOfSection, errorTime = true }); // Need to change errorTime
            }
            createVM.TitleOfSection = HttpUtility.UrlDecode(createVM.TitleOfSection);
            var createPostRequest = PostMappers.MapToRequest(createVM, userId);
            var result = await _postService.CreatePostAsync(createPostRequest);
            return result switch
            {
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                PostResult.Success => RedirectToAction("Index","Post", new { titleOfSection = HttpUtility.UrlEncode(createVM.TitleOfSection) }),
                _ => Problem("Invalid result")
            };
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(string titleOfSection, int page=1)
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
            var getIndexPostRequest = new GetIndexPostRequest
            {
                TitleOfSection = titleOfSection,
                Page = page
            };
            var postDtoList = await _postService.GetPostItemDtosByTitleAndPageAsync(getIndexPostRequest);
            titleOfSection = HttpUtility.UrlEncode(titleOfSection);
            var postIndexItemsVMs = postDtoList.Select(PostMappers.MapToViewModel).ToList();
            var postIndexViewModel = PostMappers.MapToViewModel(postIndexItemsVMs, page, totalPages, titleOfSection);
            return View(postIndexViewModel);
        }
    }
}
