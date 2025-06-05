using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;

namespace Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly IUserService _userService;
        private readonly IReportService _reportService;
        public ReportController(IUserService userService, IReportService reportService)
        {
            _userService = userService;
            _reportService = reportService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
