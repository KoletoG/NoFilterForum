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

            return View();
        }
    }
}
