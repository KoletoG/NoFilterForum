using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;
using Application.Interfaces.Services;
using Web.Mappers;

namespace Web.Controllers
{
    public class SearchController(ISearchService searchService) : Controller
    {
        private readonly ISearchService _searchService = searchService;
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(string text)
        {
            var dtos = await _searchService.GetPostsAsync(text);
            var vm = dtos.Select(SearchMapper.MapToViewModel).ToList();
            return View(vm);
        }
    }
}
