using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Web;
using Application.Interfaces.Services;
using Core.Constants;
using Core.DTOs.OutputDTOs.Reply;
using Core.Enums;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.InputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Reply;
using Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;
using Web.Helpers;
using Web.Mappers;
using Web.ViewModels.Profile;

namespace Web.Controllers
{
    public class ProfileController(IUserService userService, IReplyService replyService, IPostService postService) : Controller
    {
        private readonly IUserService _userService = userService;
        private readonly IReplyService _replyService = replyService;
        private readonly IPostService _postService = postService;
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel changeEmailViewModel, CancellationToken cancellationToken)
        {
            // EmailExists check happens in service too / CHANGE THAT
            var emailExists = await _userService.EmailExistsAsync(changeEmailViewModel.Email, cancellationToken);
            if (emailExists)
            {
                ModelState.AddModelError("emailExists", "Email already exists");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }
            var changeEmailRequest = ProfileMapper.MapToRequest(changeEmailViewModel, userId);
            var result = await _userService.ChangeEmailByIdAsync(changeEmailRequest, cancellationToken);
            return result switch
            {
                PostResult.Success => RedirectToAction(nameof(Index), new { userId }),
                PostResult.NotFound => NotFound($"User with Id: {userId} was not found."),
                PostResult.UpdateFailed => Problem(),
                PostResult.Conflict => Conflict($"The provided email: {changeEmailViewModel.Email} has already been registered"),
                _ => Problem()
            };
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangeUsername(ChangeUsernameViewModel changeUsernameViewModel, CancellationToken cancellationToken)
        {
            var usernameExists = await _userService.UsernameExistsAsync(changeUsernameViewModel.Username, cancellationToken);
            if (usernameExists)
            {
                ModelState.AddModelError("usernameExists", "Username already exists");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }
            var changeUsernameRequest = ProfileMapper.MapToRequest(changeUsernameViewModel, userId);
            var result = await _userService.ChangeUsernameByIdAsync(changeUsernameRequest,cancellationToken);
            return result switch
            {
                PostResult.Success => RedirectToAction(nameof(Index), new { userId }),
                PostResult.NotFound => NotFound($"User with Id: {userId} was not found"),
                PostResult.UpdateFailed => Problem(),
                PostResult.Conflict => Conflict($"The provided username: {changeUsernameViewModel.Username} has already been used"),
                _ => Problem()
            };
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangeBio(ChangeBioViewModel changeBioViewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId is null)
            {
                return Unauthorized();
            }
            var changeUserRequest = ProfileMapper.MapToRequest(changeBioViewModel,currentUserId);
            var result = await _userService.ChangeBioAsync(changeUserRequest, cancellationToken);
            return result switch
            {
                PostResult.Success => RedirectToAction(nameof(Index), new {userId= currentUserId }),
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> UpdateImage(UpdateImageViewModel changeImageViewModel, CancellationToken cancellationToken)
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
            var changeImageRequest = ProfileMapper.MapToRequest(changeImageViewModel, userId);
            var result = await _userService.UpdateImageAsync(changeImageRequest, cancellationToken);
            return result switch
            {
                PostResult.Success => NoContent(),
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
        }
        private static PageTotalPagesDTO GetPageTotalPages(IReadOnlyCollection<ReplyItemDto> replyItems, IReadOnlyCollection<ProfilePostDto> profilePostItems, int page)
        {
            var totalCount = replyItems.Count + profilePostItems.Count; 
            return PageUtility.GetPageTotalPagesDTO(page,totalCount);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(string userId,CancellationToken cancellationToken, int page=1)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest($"Id cannot be null or empty");
            }
            if (DefaultUserHelper.IsDefaultUserId(userId))
            {
                return Forbid();
            }
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var getProfileDtoRequest = ProfileMapper.MapToRequest(userId, currentUserId);
            var resultUser = await _userService.GetProfileDtoByUserIdAsync(getProfileDtoRequest, cancellationToken);
            if (resultUser.GetResult != GetResult.Success)
            {
                return resultUser.GetResult switch
                {
                    GetResult.NotFound => NotFound(),
                    GetResult.Problem => Problem(),
                    GetResult.Forbid => Forbid(),
                    _ => Problem()
                };
            }
            var replyDtoList = await _replyService.GetListReplyItemDtoAsync(userId, cancellationToken);
            var postDtoList = await _postService.GetListProfilePostDtoAsync(userId,cancellationToken);

            var pageTotalPagesDto = GetPageTotalPages(replyDtoList, postDtoList, page);

            var replyViewModelList = replyDtoList.Select(ProfileMapper.MapToViewModel).ToList();
            var postViewModelList = postDtoList.Select(ProfileMapper.MapToViewModel).ToList();
            var dictionary = DateHelper.OrderDates(postDtoList, replyViewModelList,page, PostConstants.PostsPerSection);
            var profileUserViewModel = ProfileMapper.MapToViewModel(resultUser.UserDto!); // cannot be null as we checked it earlier
            var profileViewModel = ProfileMapper.MapToViewModel(postViewModelList,
                replyViewModelList,
                resultUser.IsSameUser,
                profileUserViewModel, 
                dictionary, 
                pageTotalPagesDto);
            return View(profileViewModel);
        }
    }
}