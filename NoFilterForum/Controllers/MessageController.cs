using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
