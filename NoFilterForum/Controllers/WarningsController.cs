using Microsoft.AspNetCore.Authorization;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;
using Core.Constants;
using Core.Enums;
using NoFilterForum.Infrastructure.Services;
using System.Security.Claims;
using Web.Mappers;
using Web.ViewModels.Warning;
using System.Runtime.InteropServices;

namespace Web.Controllers
{
    public class WarningsController(IWarningService warningService) : Controller
    {
        private readonly IWarningService _warningService = warningService;
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Details(string id, CancellationToken cancellationToken)
        {
            id = HttpUtility.UrlDecode(id);
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Id cannot be null");
            }
            var warningsDto = await _warningService.GetWarningsContentDtosByUserIdAsync(id, cancellationToken);
            var warningsVM = warningsDto.Select(WarningMapper.MapToViewModel).ToList();
            return View(warningsVM);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateWarningViewModel createWarningViewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            var createWarningRequest = WarningMapper.MapToRequest(createWarningViewModel);
            var result = await _warningService.AddWarningAsync(createWarningRequest, cancellationToken);
            return result switch
            {
                PostResult.Success => RedirectToAction(nameof(ProfileController.Index), "Profile", new { userId = createWarningRequest.UserId }),
                PostResult.UpdateFailed => Problem(),
                PostResult.NotFound => NotFound($"User with Id: {createWarningViewModel.UserId} was not found"),
                _ => Problem()
            };
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }
            var result = await _warningService.AcceptWarningsAsync(userId);
            return result switch
            {
                PostResult.UpdateFailed => Problem(),
                PostResult.Success => RedirectToAction(nameof(NotificationsController.Index), "Notifications"),
                _ => Problem()
            };
        }
    }
}
