using Core.Enums;
using System.Web;
using Core.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Core.Models.GetViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Security.Claims;
using Ganss.Xss;

namespace Web.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostDto createDto)
        {
            if (!ModelState.IsValid)
            {
                string errorsJson = JsonSerializer.Serialize(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                errorsJson = HttpUtility.HtmlEncode(errorsJson);
                return RedirectToAction("Index", "Home", new { errors = errorsJson });
            }
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            if (await _postService.HasTimeoutAsync(userId))
            {
                return RedirectToAction("PostsMain", "Home", new { title = createDto.TitleOfSection, errorTime = true }); // Need to change errorTime
            }
            createDto.TitleOfSection = HttpUtility.UrlDecode(createDto.TitleOfSection);
            var result = await _postService.CreatePostAsync(createDto, userId);
            return result switch
            {
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                PostResult.Success => RedirectToAction("PostsMain", new { title = HttpUtility.UrlEncode(createDto.TitleOfSection) })
            };
        }
    }
}
