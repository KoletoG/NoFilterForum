using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using Application.Interfaces.Services;
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
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ICacheService _cacheService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;
        private readonly UserManager<UserDataModel> _userManager;
        private readonly SignInManager<UserDataModel> _signInManager;
        private readonly IHtmlSanitizer _htmlSanitizer;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserService(ICacheService cacheService, IHtmlSanitizer htmlSanitizer, IWebHostEnvironment webHostEnvironment, UserManager<UserDataModel> userManager, SignInManager<UserDataModel> signInManager, IUnitOfWork unitOfWork, ILogger<UserService> logger)
        {
            _cacheService = cacheService;
            _htmlSanitizer = htmlSanitizer;
            _signInManager = signInManager;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        // Add paging
        public async Task<IEnumerable<UserForAdminPanelDto>> GetAllUsersWithoutDefaultAsync(CancellationToken cancellationToken) => await _cacheService.TryGetValue<IReadOnlyCollection<UserForAdminPanelDto>>("usersListNoDefault", _unitOfWork.Users.GetUserItemsForAdminDtoAsync, cancellationToken) ?? [];
        public async Task<bool> IsAdminOrVIPAsync(string userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user is null)
            {
                return false;
            }
            return await _userManager.IsInRoleAsync(user, nameof(UserRoles.VIP)) || await _userManager.IsInRoleAsync(user, nameof(UserRoles.Admin));
        }
        public async Task<bool> IsAdminAsync(string userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user is null)
            {
                return false;
            }
            return await _userManager.IsInRoleAsync(user, nameof(UserRoles.Admin));
        }
        public async Task ApplyRoleAsync(UserDataModel user)
        {
            if (!await _userManager.IsInRoleAsync(user, nameof(UserRoles.VIP)) && !await _userManager.IsInRoleAsync(user, nameof(UserRoles.Admin)))
            {
                var role = user.PostsCount switch
                {
                    > 500 => UserRoles.Dinosaur,
                    > 20 => UserRoles.Regular,
                    _ => UserRoles.Newbie
                };
                switch (role)
                {
                    case UserRoles.Dinosaur: await _userManager.AddToRoleAsync(user, nameof(UserRoles.Dinosaur)); break;
                    case UserRoles.Regular: await _userManager.AddToRoleAsync(user, nameof(UserRoles.Regular)); break;
                    case UserRoles.Newbie: await _userManager.AddToRoleAsync(user, nameof(UserRoles.Newbie)); break;
                    default: break;
                }
                user.ChangeRole(role);
            }
        }
        public async Task<CurrentUserReplyIndexDto?> GetCurrentUserReplyIndexDtoByIdAsync(string userId, CancellationToken cancellationToken) => await _cacheService.TryGetValue<CurrentUserReplyIndexDto?>($"currentUserReplyIndexDtoById_{userId}", _unitOfWork.Users.GetCurrentUserReplyIndexDtoByIdAsync, userId, cancellationToken);
        public async Task<ProfileDto> GetProfileDtoByUserIdAsync(GetProfileDtoRequest getProfileDtoRequest)
        {
            // CHECK ON THIS TOMORROW
            var user = await _unitOfWork.Users.GetByIdAsync(getProfileDtoRequest.UserId);
            if (user is null)
            {
                return new(GetResult.NotFound, default, null);
            }
            var profileUserDto = await _cacheService.TryGetValue<ProfileUserDto?>($"profileUserDtoById_{user.Id}", _unitOfWork.Users.GetProfileUserDtoByIdAsync, user.Id);
            return new(GetResult.Success, getProfileDtoRequest.UserId == getProfileDtoRequest.CurrentUserId, profileUserDto);
        }
        public bool IsDefaultUserId(string id) => id == UserConstants.DefaultUser.Id; // Change to static method
        public async Task<PostResult> ChangeUsernameByIdAsync(ChangeUsernameRequest changeUsernameRequest, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(changeUsernameRequest.UserId);
                if (user is null)
                {
                    return PostResult.NotFound;
                }
                var normalizedUsername = _userManager.NormalizeName(changeUsernameRequest.Username);
                if (await _unitOfWork.Users.ExistNormalizedUsername(normalizedUsername, cancellationToken))
                {
                    return PostResult.Conflict;
                }
                user.ChangeUsername(changeUsernameRequest.Username);
                user.ChangeNormalizedUsername(normalizedUsername);
                _unitOfWork.Users.Update(user);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                await _userManager.UpdateSecurityStampAsync(user);
                await _signInManager.SignOutAsync();
                await _signInManager.SignInAsync(user, isPersistent: false);
                return PostResult.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Username wasn't updated for user with Id: {UserId}", changeUsernameRequest.UserId);
                return PostResult.UpdateFailed;
            }
        }
        public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken) => await _unitOfWork.Users.UsernameExistsAsync(username, cancellationToken);
        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken) => await _unitOfWork.Users.EmailExistsAsync(email, cancellationToken);
        public async Task<PostResult> ChangeEmailByIdAsync(ChangeEmailRequest changeEmailRequest, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(changeEmailRequest.UserId);
                if (user is null)
                {
                    return PostResult.NotFound;
                }
                var normalizedEmail = _userManager.NormalizeEmail(changeEmailRequest.Email);
                if(await _unitOfWork.Users.ExistNormalizedEmailAsync(normalizedEmail, cancellationToken))
                {
                    return PostResult.Conflict;
                }
                user.ChangeEmail(changeEmailRequest.Email); // Needs Change email token
                user.ChangeNormalizedEmail(normalizedEmail);
                _unitOfWork.Users.Update(user);
                await _unitOfWork.CommitAsync(cancellationToken); // ADD TOKENS FOR EMAIL CHANGE
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                await _userManager.UpdateSecurityStampAsync(user);
                await _signInManager.SignOutAsync();
                await _signInManager.SignInAsync(user, isPersistent: false);
                return PostResult.Success;
            }
            catch(OperationCanceledException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Updating email for user with Id: {UserId} was cancelled", changeEmailRequest.UserId);
                return PostResult.UpdateFailed;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Email wasn't updated for user with Id: {UserId}", changeEmailRequest.UserId);
                return PostResult.UpdateFailed;
            }
        }
        public async Task<bool> AnyNotConfirmedUsersAsync(CancellationToken cancellationToken) => await _unitOfWork.Users.ExistsByNotConfirmedAsync(cancellationToken);
        public async Task<IReadOnlyCollection<UsersReasonsDto>> GetAllUnconfirmedUsersAsync(CancellationToken cancellationToken) => await _cacheService.TryGetValue<IReadOnlyCollection<UsersReasonsDto>>("listUnconfirmedUserDtos", _unitOfWork.Users.GetAllUnconfirmedUserDtosAsync, cancellationToken) ?? [];
        public async Task<UserDataModel?> GetUserByIdAsync(string id) => await _unitOfWork.Users.GetByIdAsync(id);
        public async Task<PostResult> ConfirmUserAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user is null)
            {
                return PostResult.NotFound;
            }
            if (user.IsConfirmed)
            {
                _logger.LogInformation("User with Id: {UserId} has already been confirmed", userId);
                return PostResult.Success;
            }
            user.Confirm();
            try
            {
                await _unitOfWork.RunPOSTOperationAsync<UserDataModel>(_unitOfWork.Users.Update, user, cancellationToken);
                return PostResult.Success;
            }
            catch(OperationCanceledException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Confirming user with Id: {UserId} was cancelled", userId);
                return PostResult.UpdateFailed;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "User with {UserId} was not confirmed", userId);
                return PostResult.UpdateFailed;
            }
        }
        public async Task<PostResult> BanUserByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user is null)
            {
                return PostResult.NotFound;
            }
            var posts = await _unitOfWork.Posts.GetAllByUserIdAsync(userId, cancellationToken);
            var replies = await _unitOfWork.Replies.GetAllByUserIdAsync(userId, cancellationToken);
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
                await _unitOfWork.RunPOSTOperationAsync(
                    _unitOfWork.Posts.UpdateRange, posts,
                    _unitOfWork.Replies.UpdateRange, replies,
                    _unitOfWork.Users.Delete, user,
                    cancellationToken
                    );
                return PostResult.Success;
            }
            catch(OperationCanceledException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Banning user with Id: {UserId} was cancelled", userId);
                return PostResult.UpdateFailed;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Failed to ban user with Id: {UserId}", userId);
                return PostResult.UpdateFailed;
            }
        }
        public async Task<PostResult> ChangeBioAsync(ChangeBioRequest changeBioRequest)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(changeBioRequest.UserId);
            if (user is null)
            {
                return PostResult.NotFound;
            }
            string sanitizedFormattedBio = _htmlSanitizer.Sanitize(changeBioRequest.Bio);
            sanitizedFormattedBio = TextFormatter.FormatBody(changeBioRequest.Bio); // Move that
            if (user.Bio == sanitizedFormattedBio)
            {
                return PostResult.Success;
            }
            user.ChangeBio(sanitizedFormattedBio);
            try
            {
                await _unitOfWork.RunPOSTOperationAsync(_unitOfWork.Users.Update, user);
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
            var invalidChars = Path.GetInvalidFileNameChars();
            imageFileName = Path.GetFileName(_htmlSanitizer.Sanitize(imageFileName))
                                .Replace(' ', '_')
                                .ToLower();
            imageFileName = new string(imageFileName.Where(c => !invalidChars.Contains(c)).ToArray());
            return string.Concat(NanoidDotNet.Nanoid.Generate(), "_", imageFileName);
        }
        private string GetImageUrl(string imageName) => Path.Combine("images", imageName);
        public async Task<PostResult> UpdateImageAsync(UpdateImageRequest updateImageRequest)
        {
            var currentUser = await _unitOfWork.Users.GetByIdAsync(updateImageRequest.UserId);
            if (currentUser is null)
            {
                return PostResult.NotFound;
            }
            var fileUrl = GetImageFileUrl(updateImageRequest.Image.FileName);
            var currentUserImageUrl = currentUser.ImageUrl;
            currentUser.ChangeImageUrl(GetImageUrl(fileUrl));
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                using (var stream = new FileStream(Path.Combine(_webHostEnvironment.WebRootPath, "images", fileUrl), FileMode.Create))
                {
                    await updateImageRequest.Image.CopyToAsync(stream);
                } // THINK OF ANOTHER WAY
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
