﻿using System.Runtime.InteropServices.Marshalling;
using Core.Constants;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Admin;
using Core.Models.DTOs.OutputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Reply;
using Core.Utility;
using Ganss.Xss;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;
using static System.Net.Mime.MediaTypeNames;

namespace NoFilterForum.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;
        private readonly UserManager<UserDataModel> _userManager;
        private readonly SignInManager<UserDataModel> _signInManager;
        private readonly IHtmlSanitizer _htmlSanitizer; 
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserService(IMemoryCache memoryCache,IHtmlSanitizer htmlSanitizer,IWebHostEnvironment webHostEnvironment, UserManager<UserDataModel> userManager, SignInManager<UserDataModel> signInManager, IUnitOfWork unitOfWork, ILogger<UserService> logger)
        {
            _memoryCache = memoryCache;
            _htmlSanitizer = htmlSanitizer;
            _signInManager = signInManager;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        // Add paging
        public async Task<List<UserForAdminPanelDto>> GetAllUsersWithoutDefaultAsync()
        {
            if (!_memoryCache.TryGetValue($"usersListNoDefault", out List<UserForAdminPanelDto> users))
            {
                users = await _unitOfWork.Users.GetUserItemsForAdminDtoAsync();
                _memoryCache.Set($"usersListNoDefault", users, TimeSpan.FromMinutes(5));
            }
            return users;
        }
        public async Task<CurrentUserReplyIndexDto> GetCurrentUserReplyIndexDtoByIdAsync(string id)
        {
            return await _unitOfWork.Users.GetCurrentUserReplyIndexDtoByIdAsync(id);
        }
        public Dictionary<string,DateTime> OrderDates(List<ProfilePostDto> postItemDtos, List<ReplyItemDto> replyItemDtos, int page, int countPerPage)
        {
            Dictionary<string, DateTime> dateOrder = new Dictionary<string, DateTime>();
            foreach (var post in postItemDtos)
            {
                dateOrder[post.Id] = post.Created;
            }
            foreach (var reply in replyItemDtos)
            {
                reply.Content = TextFormatter.ReplaceLinkText(reply.Content);
                dateOrder[reply.Id] = reply.Created;
            }
            return dateOrder.OrderByDescending(x => x.Value).Skip((page - 1) * countPerPage).Take(countPerPage).ToDictionary();
        }
        public async Task<ProfileDto> GetProfileDtoByUserIdAsync(GetProfileDtoRequest getProfileDtoRequest)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(getProfileDtoRequest.UserId);
            if(user == null)
            {
                return new ProfileDto()
                {
                    GetResult = GetResult.NotFound
                };
            }
            var profileUserDto = await _unitOfWork.Users.GetProfileUserDtoByIdAsync(user.Id);
            bool isSameUser = false;
            if(getProfileDtoRequest.UserId == getProfileDtoRequest.CurrentUserId)
            {
                isSameUser = true;
            }
            return new()
            {
                GetResult = GetResult.Success,
                IsSameUser = isSameUser,
                UserDto = profileUserDto
            };
        }
        public bool IsDefaultUserId(string id)
        {
            return id == UserConstants.DefaultUser.Id;
        }
        public async Task<PostResult> ChangeUsernameByIdAsync(ChangeUsernameRequest changeUsernameRequest)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(changeUsernameRequest.UserId);
            if (user == null)
            {
                return PostResult.NotFound;
            }
            user.ChangeUsername(changeUsernameRequest.Username);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _unitOfWork.CommitTransactionAsync();
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return PostResult.Success;
                }
                else
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return PostResult.UpdateFailed;
                }
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Username wasn't updated for user with Id: {UserId}", changeUsernameRequest.UserId);
                return PostResult.UpdateFailed;
            }
        }
        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _unitOfWork.Users.UsernameExistsAsync(username);
        }
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _unitOfWork.Users.EmailExistsAsync(email);
        }
        public int GetTotalCountByPostsAndReplies(List<ReplyItemDto> replies, List<ProfilePostDto> posts)
        {
            return replies.Count + posts.Count;
        }
        public async Task<PostResult> ChangeEmailByIdAsync(ChangeEmailRequest changeEmailRequest)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(changeEmailRequest.UserId);
            if (user == null)
            {
                return PostResult.NotFound;
            }
            user.ChangeEmail(changeEmailRequest.Email); // Needs Change email token
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _unitOfWork.CommitTransactionAsync();
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return PostResult.Success;
                }
                else
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return PostResult.UpdateFailed;
                }
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Email wasn't updated for user with Id: {UserId}", user.Id);
                return PostResult.UpdateFailed;
            }
        }
        public async Task<bool> AnyNotConfirmedUsersAsync()
        {
            return await _unitOfWork.Users.ExistsByNotConfirmedAsync();
        }
        public async Task<List<UsersReasonsDto>> GetAllUnconfirmedUsersAsync()
        {
            return await _unitOfWork.Users.GetAllUnconfirmedUserDtosAsync();
        }
        public async Task<UserDataModel> GetUserByIdAsync(string id)
        {
            return await _unitOfWork.Users.GetByIdAsync(id);
        }
        public async Task<PostResult> ConfirmUserAsync(string userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                return PostResult.NotFound;
            }
            if (user.IsConfirmed)
            {
                _logger.LogInformation("User with Id: {UserId} has already been confirmed",userId);
                return PostResult.Success;
            }
            user.Confirm();
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.Users.Update(user);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return PostResult.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "User with {UserId} was not confirmed.",userId);
                return PostResult.UpdateFailed;
            }
        }
        public async Task<PostResult> BanUserByIdAsync(string userId)
        {

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                return PostResult.NotFound;
            }
            var posts = await _unitOfWork.Posts.GetAllByUserIdAsync(userId);
            var replies = await _unitOfWork.Replies.GetAllByUserIdAsync(userId);
            foreach (var post in posts)
            {
                post.SetDefaultUser();
            }
            foreach (var reply in replies)
            {
                reply.SetDefaultUser();
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Posts.UpdateRangeAsync(posts);
                await _unitOfWork.Replies.UpdateRangeAsync(replies);
                await _unitOfWork.Users.DeleteAsync(user);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return PostResult.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Failed to ban user with Id: {UserId}",userId);
                return PostResult.UpdateFailed;
            }
        }
        public async Task<bool> IsAdminRoleByIdAsync(string userId)
        {
            var userRole = await _unitOfWork.Users.GetUserRoleIdAsync(userId);
            if(userRole == UserRoles.Admin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<PostResult> ChangeBioAsync(ChangeBioRequest changeBioRequest)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(changeBioRequest.UserId);
            if (user == null)
            {
                return PostResult.NotFound;
            }
            changeBioRequest.Bio = _htmlSanitizer.Sanitize(changeBioRequest.Bio);
            changeBioRequest.Bio = TextFormatter.FormatBody(changeBioRequest.Bio);
            if(user.Bio == changeBioRequest.Bio)
            {
                return PostResult.Success;
            }
            user.ChangeBio(changeBioRequest.Bio);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.Users.Update(user);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return PostResult.Success;
            }
            catch (Exception ex) 
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "User's bio with Id: {UserId} was not changed", changeBioRequest.UserId);
                return PostResult.UpdateFailed;
            }
        }
        private string GetImageFileUrl(string imageFileName)
        {
            imageFileName = Path.GetFileName(_htmlSanitizer.Sanitize(imageFileName));
            imageFileName = imageFileName.Replace(' ', '_');
            imageFileName = imageFileName.ToLower(); 
            var invalidChars = Path.GetInvalidFileNameChars();
            imageFileName = new string(imageFileName.Where(c => !invalidChars.Contains(c)).ToArray());
            string randomImgUrl = NanoidDotNet.Nanoid.Generate() + "_" + imageFileName;
            return randomImgUrl;
        }
        private string GetImageUrl(string imageName)
        {
            return Path.Combine("images", imageName);
        }
        public async Task<PostResult> UpdateImageAsync(UpdateImageRequest updateImageRequest)
        {
            var currentUser = await _unitOfWork.Users.GetByIdAsync(updateImageRequest.UserId);
            if (currentUser == null) 
            {
                return PostResult.NotFound;
            }
            var fileUrl = GetImageFileUrl(updateImageRequest.Image.FileName);
            var currentUserImageUrl = currentUser.ImageUrl;
            currentUser.ChangeImageUrl(GetImageUrl(fileUrl));
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                using (var stream = new FileStream(Path.Combine(_webHostEnvironment.WebRootPath,"images", fileUrl), FileMode.Create))
                {
                    await updateImageRequest.Image.CopyToAsync(stream);
                }
                _unitOfWork.Users.Update(currentUser);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                if (currentUserImageUrl != Path.Combine("images", "defaultimage.gif")) // Avoid hardcoding
                {
                    var pathToDelete = Path.Combine(_webHostEnvironment.WebRootPath, currentUserImageUrl);
                    System.IO.File.Delete(pathToDelete);
                }
                return PostResult.Success;
            }
            catch (Exception ex) 
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Image of user with Id: {UserId} was not changed", updateImageRequest.UserId);
                return PostResult.UpdateFailed;
            }
        }
    }
}
