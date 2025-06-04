using System.Security.Claims;
using Core.Enums;
using Core.Models.DTOs.InputDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;
using Web.Mappers;
using Web.ViewModels.Profile;

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
            var emailExists = await _userService.EmailExistsAsync(changeEmailViewModel.Email);
            if (emailExists)
            {
                ModelState.AddModelError(nameof(changeEmailViewModel.Email), "Email already exists");
                return View(changeEmailViewModel); // Change this to redirect to Profile after Clean Architecture
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
            }; // USE MAPPER
            var result = await _userService.ChangeEmailByIdAsync(changeEmailRequest);
            return result switch
            {
                PostResult.Success => RedirectToAction("Index", "Home"),
                PostResult.NotFound => NotFound($"User with Id: {userId} was not found."),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangeUsername(ChangeUsernameViewModel changeUsernameViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var usernameExists = await _userService.UsernameExistsAsync(changeUsernameViewModel.Username);
            if (usernameExists)
            {
                ModelState.AddModelError(nameof(changeUsernameViewModel.Username), "Username already exists");
                return View(changeUsernameViewModel); // Change this to redirect to Profile after Clean Architecture
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var changeUsernameRequest = new ChangeUsernameRequest
            {
                Username = changeUsernameViewModel.Username,
                UserId = userId
            }; // USE MAPPER
            var result = await _userService.ChangeUsernameByIdAsync(changeUsernameRequest);
            return result switch
            {
                PostResult.Success => RedirectToAction("Index", "Home"),
                PostResult.NotFound => NotFound($"User with Id: {userId} was not found"),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangeBio(ChangeBioViewModel changeBioViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Unauthorized();
            }
            var changeUserRequest = ProfileMapper.MapToRequest(changeBioViewModel,currentUserId);
            var result = await _userService.ChangeBioAsync(changeUserRequest);
            return result switch
            {
                PostResult.Success => RedirectToAction("Profile", "Home", new { userName = User.Identity.Name }),
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangeImage(ChangeImageViewModel changeImageViewModel)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            return Ok();
        }
    }
}
