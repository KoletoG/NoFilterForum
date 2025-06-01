using Microsoft.AspNetCore.Authorization;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;
using Web.Mappers.Warnings;
using Core.Constants;
using Core.Enums;
using NoFilterForum.Infrastructure.Services;
using Web.ViewModels;
using System.Security.Claims;

namespace Web.Controllers
{
    public class WarningsController : Controller
    {
        private readonly IWarningService _warningService;
        private readonly IUserService _userService;
        public WarningsController(IWarningService warningService, IUserService userService)
        {
            _warningService = warningService;
            _userService = userService;
        }
        [Authorize]
        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Details(string id)
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
            var warningsVM = warningsDto.Select(WarningMappers.MapToViewModel).ToList();
            return View(warningsVM);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateWarningViewModel createWarningViewModel)
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)); // Change that
            }
            var createWarningRequest = WarningMappers.MapToRequest(createWarningViewModel);
            var result = await _warningService.AddWarningAsync(createWarningRequest);
            return result switch
            {
                PostResult.Success => RedirectToAction("Profile", "Home", new { userName = "CHANGE" }), // Change to ID
                PostResult.UpdateFailed => Problem(),
                PostResult.NotFound => NotFound(createWarningViewModel.UserId),
                _ => Problem()
            };
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var result = await _warningService.AcceptWarningsAsync(userId);
            return result switch
            {
                PostResult.UpdateFailed => Problem(),
                PostResult.Success => RedirectToAction("Index", "Notification")
            };
        }
    }
}
