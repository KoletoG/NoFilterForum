using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Web;
using Core.Constants;
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
using Web.Mappers;
using Web.ViewModels.Profile;

namespace Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly IReplyService _replyService;
        private readonly IPostService _postService;
        public ProfileController(IUserService userService,IPostService postService, IReplyService replyService)
        {
            _postService = postService;
            _userService = userService;
            _replyService = replyService;
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
            var changeEmailRequest = ProfileMapper.MapToRequest(changeEmailViewModel, userId);
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
            var changeUsernameRequest = ProfileMapper.MapToRequest(changeUsernameViewModel, userId);
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
        public async Task<IActionResult> UpdateImage(UpdateImageViewModel changeImageViewModel)
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
            var changeImageRequest = ProfileMapper.MapToRequest(changeImageViewModel, userId);
            var result = await _userService.UpdateImageAsync(changeImageRequest);
            return result switch
            {
                PostResult.Success => NoContent(),
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(string userId, int page=1)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest($"Id cannot be null or empty");
            }
            if (_userService.IsDefaultUserId(userId))
            {
                return Forbid();
            }
            var getProfileDtoRequest = ProfileMapper.MapToRequest(userId, User.FindFirstValue(ClaimTypes.NameIdentifier));
            var resultUser = await _userService.GetProfileDtoByUserIdAsync(getProfileDtoRequest);
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
            var profileUserViewModel = ProfileMapper.MapToViewModel(resultUser.UserDto);
            var replyDtoRequest = ReplyMapper.MapToRequest(userId);
            List<ReplyItemDto> replyDtoList = await _replyService.GetListReplyItemDtoAsync(replyDtoRequest);
            var postDtoRequest = PostMappers.MapToRequest(userId);
            List<ProfilePostDto> postDtoList = await _postService.GetListProfilePostDtoAsync(postDtoRequest);
            var totalCount = _userService.GetTotalCountByPostsAndReplies(replyDtoList, postDtoList);
            int totalPageCount = PageUtility.GetTotalPagesCount(totalCount,PostConstants.PostsPerSection);
            page = PageUtility.ValidatePageNumber(page,totalPageCount);
            var replyViewModelList = replyDtoList.Select(ProfileMapper.MapToViewModel).ToList();
            var postViewModelList = postDtoList.Select(ProfileMapper.MapToViewModel).ToList();
            var dictionary = _userService.OrderDates(postDtoList, replyDtoList,page, PostConstants.PostsPerSection);
            var profileViewModel = ProfileMapper.MapToViewModel(postViewModelList,
                replyViewModelList,
                resultUser.IsSameUser,
                profileUserViewModel, 
                page, 
                dictionary, 
                totalPageCount);
            return View(profileViewModel);
        }
    }
}
