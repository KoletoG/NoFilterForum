using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Web;
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
            var emailExists = await _userService.EmailExistsAsync(changeEmailViewModel.Email);
            if (emailExists)
            {
                ModelState.AddModelError(nameof(changeEmailViewModel.Email), "Email already exists");
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
            var result = await _userService.ChangeEmailByIdAsync(changeEmailRequest);
            return result switch
            {
                PostResult.Success => RedirectToAction(nameof(Index), new {userId}),
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
            var usernameExists = await _userService.UsernameExistsAsync(changeUsernameViewModel.Username);
            if (usernameExists)
            {
                ModelState.AddModelError(nameof(changeUsernameViewModel.Username), "Username already exists");
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
            var result = await _userService.ChangeUsernameByIdAsync(changeUsernameRequest);
            return result switch
            {
                PostResult.Success => RedirectToAction(nameof(Index), new { userId }),
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
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId is null)
            {
                return Unauthorized();
            }
            var changeUserRequest = ProfileMapper.MapToRequest(changeBioViewModel,currentUserId);
            var result = await _userService.ChangeBioAsync(changeUserRequest);
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
        public async Task<IActionResult> UpdateImage(UpdateImageViewModel changeImageViewModel)
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
            var result = await _userService.UpdateImageAsync(changeImageRequest);
            return result switch
            {
                PostResult.Success => NoContent(),
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
        }
        private PageTotalPagesDTO GetPageTotalPages(List<ReplyItemDto> replyItems, List<ProfilePostDto> profilePostItems, int page)
        {
            var totalCount = replyItems.Count + profilePostItems.Count; 
            int totalPageCount = PageUtility.GetTotalPagesCount(totalCount, PostConstants.PostsPerSection);
            page = PageUtility.ValidatePageNumber(page, totalPageCount);
            return new(page, totalPageCount);
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

            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var getProfileDtoRequest = ProfileMapper.MapToRequest(userId, currentUserId);
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

            var replyDtoRequest = ReplyMapper.MapToRequest(userId);
            List<ReplyItemDto> replyDtoList = await _replyService.GetListReplyItemDtoAsync(replyDtoRequest);

            var postDtoRequest = PostMappers.MapToRequest(userId);
            List<ProfilePostDto> postDtoList = await _postService.GetListProfilePostDtoAsync(postDtoRequest);

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