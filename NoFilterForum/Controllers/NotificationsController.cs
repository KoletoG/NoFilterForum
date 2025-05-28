using System.Security.Claims;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;

namespace Web.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly INotificationService _notificationService;
        public NotificationsController(INotificationService notificationService) 
        {
            _notificationService = notificationService;
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
                PostResult.Success => RedirectToAction("Notifications", "Home"), // Change that to ("Notifications or Index","Notifications")
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

        }
    }
}
