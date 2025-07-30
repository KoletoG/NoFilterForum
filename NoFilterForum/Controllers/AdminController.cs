using System.Runtime.InteropServices;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NoFilterForum.Core.Models;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Core.Models.ViewModels;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Infrastructure.Data;
using NoFilterForum.Infrastructure.Services;
using Core.Enums;
using Core.Constants;
using Web.ViewModels;
using Core.Models.DTOs.OutputDTOs;
using Web.Mappers;
using Web.ViewModels.Admin;
using Core.Models.DTOs.InputDTOs.Admin;

namespace Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        public AdminController(
            IReportService reportService,
            IUserService userService,
            IPostService postService)
        {
            _reportService = reportService;
            _userService = userService;
            _postService = postService;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PinPost(PinPostViewModel pinPostViewModel)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            var result = await _postService.PinPostAsync(pinPostViewModel.PostId);
            return result switch
            {
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                PostResult.Success => NoContent(),
                _ => Problem("Unknown result.")
            };
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("Reasons")]
        public async Task<IActionResult> Reasons()
        {
            var users = await _userService.GetAllUnconfirmedUsersAsync();
            var usersVM = users.Select(AdminMappers.MapToViewModel).ToList();
            var reasonsViewModel = AdminMappers.MapToViewModel(usersVM);
            return View(reasonsViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmUser(ConfirmUserViewModel confirmUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            var result = await _userService.ConfirmUserAsync(confirmUserViewModel.UserId);
            return result switch
            {
                PostResult.Success => RedirectToAction(nameof(Reasons)),
                PostResult.NotFound => NotFound(confirmUserViewModel.UserId),
                PostResult.UpdateFailed => Problem(),
                _ => Problem("Unknown result")
            };
        }
        [Authorize(Roles ="Admin")]
        [HttpGet]
        [Route("Adminpanel")]
        public async Task<IActionResult> Index()
        {
            var usersDto = await _userService.GetAllUsersWithoutDefaultAsync();
            var userViewModel = usersDto.Select(AdminMappers.MapToViewModel).ToList();
            bool notConfirmedExist = await _userService.AnyNotConfirmedUsersAsync();
            bool hasReports = await _reportService.AnyReportsAsync();
            var adminPanelVM = AdminMappers.MapToViewModel(userViewModel, hasReports, notConfirmedExist);
            return View(adminPanelVM);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BanUser(BanUserViewModel banUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            var result = await _userService.BanUserByIdAsync(banUserViewModel.Id);
            return result switch
            {
                PostResult.Success => RedirectToAction(nameof(Index)),
                PostResult.NotFound => NotFound(banUserViewModel.Id),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
        }
    }
}
