using System.Security.Claims;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.ViewModels;
using Web.Mappers;
using Web.ViewModels;
using Web.ViewModels.Notifications;

namespace Web.Controllers
{
    public class NotificationsController(INotificationService notificationService, IWarningService warningService): Controller
    {
        private readonly INotificationService _notificationService = notificationService;
        private readonly IWarningService _warningService = warningService;
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CancellationToken cancellationToken) // Might change to read with IsMarked property in NotificatonsDataModel
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }
            var result = await _notificationService.DeleteByUserIdAsync(userId, cancellationToken);
            return result switch
            {
                PostResult.Success => RedirectToAction(nameof(Index)),
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }
            var notificationsDtoList = await _notificationService.GetNotificationsDtosByUserIdAsync(userId, cancellationToken);
            var warningsContentDtosList = await _warningService.GetWarningsContentDtosByUserIdAsync(userId, cancellationToken);
            var notificationsItemsViewModels = notificationsDtoList.Select(NotificationMapper.MapToViewModel).ToList();
            var warningsItemViewModel = warningsContentDtosList.Select(WarningMapper.MapToViewModel).ToList();
            return View(new NotificationViewModel(warningsItemViewModel, notificationsItemsViewModels));
        }
    }
}
