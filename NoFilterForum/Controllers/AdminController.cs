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
using Core.Models.ViewModels;
using Web.ViewModels;
using Core.Models.DTOs.OutputDTOs;
using Web.Mappers.Admin;

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
        public async Task<IActionResult> DeleteReport(string id)
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return Forbid();
            }
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Id cannot be null");
            }
            var result = await _reportService.DeleteReportByIdAsync(id);
            return result switch
            {
                PostResult.Success => RedirectToAction("Reports"),
                PostResult.NotFound => NotFound(id),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
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
        [ResponseCache(Duration =30,Location = ResponseCacheLocation.Any)]
        [Route("Reports")]
        public async Task<IActionResult> Reports()
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return Forbid();
            }
            var reports = await _reportService.GetAllReportsAsync();
            return View(new ReportsViewModel(reports)); // needs to change
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
            var users = await _userService.GetAllUsersWithoutDefaultAsync();
            var userViewModel = users.Select(AdminMappers.MapToViewModel).ToList();
            bool notConfirmedExist = await _userService.AnyNotConfirmedUsersAsync();
            bool hasReports = await _reportService.AnyReportsAsync();
            return View(new AdminPanelViewModel(userViewModel, hasReports,notConfirmedExist));
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GiveWarning(GiveWarningViewModel giveWarningRequest)
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x=>x.Errors).Select(x=>x.ErrorMessage));
            }
            var user = await _userService.GetUserWithWarningsByIdAsync(giveWarningRequest.UserId);
            if (user == null)
            {
                return NotFound(giveWarningRequest.UserId);
            }
            var result = await _warningService.AddWarningAsync(giveWarningRequest.Content, user);
            return result switch
            {
                PostResult.Success => RedirectToAction("Profile", "Home", new { userName = user.UserName }),
                PostResult.UpdateFailed => Problem(),
                PostResult.NotFound => NotFound(giveWarningRequest.UserId),
                _ => Problem()
            };
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
        [Authorize]
        [Route("WarningsOfUserId-{id}")]
        [ResponseCache(Duration =60,Location =ResponseCacheLocation.Any)]
        public async Task<IActionResult> ShowWarnings(string id)
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return Forbid();
            }
            id = HttpUtility.UrlDecode(id);
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Id cannot be null");
            }
            var warningsDto = await _warningService.GetWarningsByUserIdAsync(id);
            var warningsVM = warningsDto.Select(dto => new ShowWarningsViewModel
            {
                Content = dto.Content
            }).ToList();
            return View(warningsVM);
        }
    }
}
