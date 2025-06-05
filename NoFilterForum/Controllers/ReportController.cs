using Core.Constants;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;
using Web.ViewModels.Admin;
using Web.ViewModels.Report;

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
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return Forbid();
            }
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Id cannot be null");
            }
            var result = await _reportService.DeleteReportByIdAsync(id);
            return result switch
            {
                PostResult.Success => RedirectToAction("Reports"),
                PostResult.NotFound => NotFound(id),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReportViewModel createReportViewModel)
        {
            return Ok();
        }
    }
}
