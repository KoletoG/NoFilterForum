using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;
using Web.ViewModels.Admin;

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

        [Authorize]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
        [Route("Reports")]
        public async Task<IActionResult> Index()
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return Forbid();
            }
            var reports = await _reportService.GetAllReportsAsync();
            return View(new ReportsViewModel(reports)); // needs to change
        }
    }
}
