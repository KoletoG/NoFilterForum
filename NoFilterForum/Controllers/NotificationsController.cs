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
    }
}
