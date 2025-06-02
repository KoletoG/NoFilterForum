using System.Security.Claims;
using System.Threading.Tasks;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.ViewModels;
using Web.Mappers;
using Web.ViewModels.Section;

namespace Web.Controllers
{
    public class SectionController : Controller
    {
        private readonly ISectionService _sectionService;
        private readonly IUserService _userService;
        public SectionController(ISectionService sectionService, IUserService userService)
        {
            _sectionService = sectionService;
            _userService = userService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Add errors check 
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var sectionItemDtos = await _sectionService.GetAllSectionItemDtosAsync();
            bool isAdmin = await _userService.IsAdminRoleByIdAsync(userId);
            var sectionItemViewModelList = sectionItemDtos.Select(SectionMapper.MapToViewModel).ToList();
            var indexSectionViewModel = SectionMapper.MapToViewModel(sectionItemViewModelList, isAdmin);
            return View(indexSectionViewModel);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSectionViewModel createSectionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var createSectionRequest = SectionMapper.MapToRequest(createSectionViewModel, userId);
            var result = await _sectionService.CreateSectionAsync(createSectionRequest);
            return result switch
            {
                PostResult.Success => RedirectToAction("Index"),
                PostResult.Forbid => Forbid(),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok();
        }
    }
}
