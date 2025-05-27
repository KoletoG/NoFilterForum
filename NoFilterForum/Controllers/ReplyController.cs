using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ReplyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
