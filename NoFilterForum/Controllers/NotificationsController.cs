using System.Security.Claims;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.ViewModels;
using Web.ViewModels;

namespace Web.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IWarningService _warningService;
        public NotificationsController(INotificationService notificationService, IWarningService warningService) 
        {
            _notificationService = notificationService;
            _warningService = warningService;
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete() // Might change to read with IsMarked property in NotificaitonsDataModel
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var result = await _notificationService.DeleteByUserIdAsync(userId);
            return result switch
            {
                PostResult.Success => RedirectToAction("Index", "Notifications"), // Change that to ("Notifications or Index","Notifications")
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var notificationsDtoList = await _notificationService.GetNotificationsDtosByUserIdAsync(userId);
            var warningsContentDtosList = await _warningService.GetWarningsContentDtosByUserIdAsync(userId);
            var notificationsItemsViewModels = notificationsDtoList.Select(x =>new NotificationItemViewModel
            {
                PostId = x.PostId,
                PostTitle = x.PostTitle,
                UserFromUsername = x.UserFromUsername,
                ReplyContent = x.ReplyContent,
                ReplyId = x.ReplyId
            }).ToList();
            var warningsItemViewModel = warningsContentDtosList.Select(x => new WarningItemViewModel
            {
                Content = x.Content
            }).ToList();
            return View(new NotificationViewModel(warningsItemViewModel, notificationsItemsViewModels));
        }
    }
}
