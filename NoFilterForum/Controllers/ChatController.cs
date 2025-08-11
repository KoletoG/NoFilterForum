using System.Security.Claims;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ChatController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }

            return View();
        }
    }
}
