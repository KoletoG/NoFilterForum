using System.Runtime.InteropServices;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NoFilterForum.Core.Models;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Core.Models.ViewModels;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Infrastructure.Data;
using NoFilterForum.Infrastructure.Services;
using Core.Enums;
using Core.Constants;
using Web.ViewModels;
using Core.Models.DTOs.OutputDTOs;
using Web.Mappers;
using Web.ViewModels.Admin;

namespace Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private readonly IWarningService _warningService;
        public AdminController(
            IReportService reportService,
            IUserService userService,
            IPostService postService,
            IWarningService warningService)
        {
            _reportService = reportService;
            _userService = userService;
            _postService = postService;
            _warningService = warningService;
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PinPost(string postId)
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return Forbid();
            }
            if (string.IsNullOrEmpty(postId))
            {
                return BadRequest("Id cannot be null");
            }
            var result = await _postService.PinPostAsync(postId);
            return result switch
            {
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                PostResult.Success => NoContent(),
                _ => Problem("Unknown result.")
            };
        }
        [Authorize]
        [Route("Reasons")]
        public async Task<IActionResult> Reasons()
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return Forbid();
            }
            var users = await _userService.GetAllUnconfirmedUsersAsync();
            return View(new ReasonsViewModel(users)); // needs to change
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmUser(string userId)
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return Forbid();
            }
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Id cannot be null");
            }
            var result = await _userService.ConfirmUserAsync(userId);
            return result switch
            {
                PostResult.Success=> RedirectToAction("Reasons"),
                PostResult.NotFound=>NotFound(userId),
                PostResult.UpdateFailed=>Problem(),
                _ => Problem("Unknown result.")
            };
        }
        [Authorize] // make authorize with roles
        [Route("Adminpanel")]
        public async Task<IActionResult> Index()
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return Forbid();
            }
            var usersDto = await _userService.GetAllUsersWithoutDefaultAsync();
            var userViewModel = usersDto.Select(AdminMappers.MapToViewModel).ToList();
            bool notConfirmedExist = await _userService.AnyNotConfirmedUsersAsync();
            bool hasReports = await _reportService.AnyReportsAsync();
            var adminPanelVM = AdminMappers.MapToViewModel(userViewModel, hasReports, notConfirmedExist);
            return View(adminPanelVM);
        }
        
            
        // Add ModelError if something went wrong, that's for every method including creating post and reply
        // ADD ENCODING
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BanUser(string id)
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return Forbid();
            }
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Id cannot be null");
            }
            var result = await _userService.BanUserByIdAsync(id);
            return result switch
            {
                PostResult.Success => RedirectToAction(nameof(Index)),
                PostResult.NotFound => NotFound(id),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            }; 
        }
    }
}
