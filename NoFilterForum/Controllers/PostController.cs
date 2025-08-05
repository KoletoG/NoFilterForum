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

namespace Web.Controllers
{
    public class PostController(IPostService postService, ISectionService sectionService) : Controller
    {
        private readonly IPostService _postService = postService;
        private readonly ISectionService _sectionService = sectionService;
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostViewModel createVM)
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
            if (await _postService.HasTimeoutAsync(userId))
            {
                return RedirectToAction(nameof(Index), new { title = createVM.TitleOfSection, errorTime = true }); // Need to change errorTime
            }
            createVM.TitleOfSection = HttpUtility.UrlDecode(createVM.TitleOfSection);
            var createPostRequest = PostMapper.MapToRequest(createVM, userId);
            var result = await _postService.CreatePostAsync(createPostRequest);
            return result switch
            {
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                PostResult.Success => RedirectToAction(nameof(Index), new { titleOfSection = HttpUtility.UrlEncode(createVM.TitleOfSection) }),
                _ => Problem("Invalid result")
            };
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Like(LikeDislikePostViewModel likeDislikePostViewModel)
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
            var result = await _postService.LikeAsync(likeDislikeRequest);
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
        public async Task<IActionResult> Dislike(LikeDislikePostViewModel likeDislikePostViewModel)
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
            var result = await _postService.DislikeAsync(likeDislikeRequest);
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
        public async Task<IActionResult> Index(string titleOfSection, int page = 1)
        {
            titleOfSection = HttpUtility.UrlDecode(titleOfSection);
            if (!await _sectionService.ExistsSectionByTitleAsync(titleOfSection))
            {
                return NotFound($"Section with title: {titleOfSection} doesn't exist");
            }
            var pageTotalPagesDTO = await GetPagesTotalPagesDtoAsync(titleOfSection, page);
            var getIndexPostRequest = PostMapper.MapToRequest(page, titleOfSection);
            var postDtoList = await _postService.GetPostItemDtosByTitleAndPageAsync(getIndexPostRequest);
            var postIndexItemsVMs = postDtoList.Select(PostMapper.MapToViewModel);
            var postIndexViewModel = PostMapper.MapToViewModel(postIndexItemsVMs, pageTotalPagesDTO, titleOfSection);
            return View(postIndexViewModel);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeletePostViewModel deletePostViewModel)
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
            var deletePostRequest = PostMapper.MapToRequest(deletePostViewModel, userId);
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
            return RedirectToAction(nameof(Index), new { titleOfSection = sectionTitle });
        }
    }
}
