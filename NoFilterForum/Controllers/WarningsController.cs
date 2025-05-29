using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;

namespace Web.Controllers
{
    public class WarningsController : Controller
    {
        private readonly IWarningService _warningService;
        public WarningsController(IWarningService warningService)
        {
            _warningService = warningService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
