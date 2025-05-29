using Microsoft.AspNetCore.Authorization;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;
using Web.Mappers.Warnings;
using Core.Constants;

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
        [Authorize]
        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Details(string id)
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return Forbid();
            }
            id = HttpUtility.UrlDecode(id);
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Id cannot be null");
            }
            var warningsDto = await _warningService.GetWarningsByUserIdAsync(id);
            var warningsVM = warningsDto.Select(WarningMappers.MapToViewModel).ToList();
            return View(warningsVM);
        }
    }
}
