using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;
using Web.Mappers.Section;
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
    }
}
