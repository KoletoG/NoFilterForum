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
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Add errors check 
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }

            var sectionItemDtos = await _sectionService.GetAllSectionItemDtosAsync();
            bool isAdmin = await _userService.IsAdminAsync(userId);
            var sectionItemViewModelList = sectionItemDtos.Select(SectionMapper.MapToViewModel);
            var indexSectionViewModel = SectionMapper.MapToViewModel(sectionItemViewModelList, isAdmin);
            return View(indexSectionViewModel);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSectionViewModel createSectionViewModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            var createSectionRequest = SectionMapper.MapToRequest(createSectionViewModel, userId);
            var result = await _sectionService.CreateSectionAsync(createSectionRequest);
            return result switch
            {
                PostResult.Success => RedirectToAction(nameof(Index)),
                PostResult.Forbid => Forbid(),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteSectionViewModel deleteSectionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }
            if (!await _userService.IsAdminAsync(userId))
            {
                return Forbid();
            }
            var deleteSectionRequest = SectionMapper.MapToRequest(deleteSectionViewModel);
            var result = await _sectionService.DeleteSectionAsync(deleteSectionRequest);
            return result switch
            {
                PostResult.Success => RedirectToAction(nameof(Index)),
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
        }
    }
}
