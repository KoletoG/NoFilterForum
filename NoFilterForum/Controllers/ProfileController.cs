using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;

namespace Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }
    }
}
