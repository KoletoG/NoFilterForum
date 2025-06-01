using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;

namespace Web.Controllers
{
    public class SectionController : Controller
    {
        private readonly ISectionService _sectionService;
        public SectionController(ISectionService sectionService) 
        {
            _sectionService = sectionService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
