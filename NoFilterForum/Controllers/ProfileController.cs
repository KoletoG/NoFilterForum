using System.Security.Claims;
using Core.Enums;
using Core.Models.DTOs.InputDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;
using Web.ViewModels;

namespace Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel changeEmailViewModel)
        {
            if (!ModelState.IsValid) 
            { 
                return BadRequest(ModelState);
            }
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var changeEmailRequest = new ChangeEmailRequest
            {
                Email = changeEmailViewModel.Email,
                UserId = userId
            };
            var result = await _userService.ChangeEmailByIdAsync(changeEmailRequest);
            return result switch
            {
                PostResult.Success => RedirectToAction("Index", "Home"),
                PostResult.NotFound => NotFound($"User with Id: {userId} was not found."),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
        }
    }
}
