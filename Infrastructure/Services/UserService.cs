using Core.Constants;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;
        private readonly IPostService _postService;
        private readonly IReplyService _replyService;
        public UserService(IMemoryCache memoryCache, IUnitOfWork unitOfWork, ILogger<UserService> logger, IPostService postService, IReplyService replyService)
        {
            _memoryCache = memoryCache;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _postService = postService;
            _replyService = replyService;
        }
        // Add paging
        public async Task<List<UserDataModel>> GetAllUsersWithoutDefaultAsync()
        {
            if (!_memoryCache.TryGetValue($"usersListNoDefault", out List<UserDataModel> users))
            {
                users = await _unitOfWork.Users.GetAllNoDefaultAsync();
                _memoryCache.Set($"usersListNoDefault", users, TimeSpan.FromMinutes(5));
            }
            return users;
        }
        public async Task<PostResult> ChangeEmailByIdAsync(ChangeEmailRequest changeEmailRequest)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(changeEmailRequest.UserId);
            if (user == null)
            {
                return PostResult.NotFound;
            }
            user.ChangeEmail(changeEmailRequest.Email);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return PostResult.Success;
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
        public async Task<List<UserDataModel>> GetAllUnconfirmedUsersAsync()
        {
            return await _unitOfWork.Users.GetAllUnconfirmedAsync();
        }
        public async Task<UserDataModel> GetUserByIdAsync(string id)
        {
            return await _unitOfWork.Users.GetByIdAsync(id);
        }
        public async Task<UserDataModel> GetUserWithWarningsByIdAsync(string userId)
        {
            return await _unitOfWork.Users.GetUserWithWarningsByIdAsync(userId);
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
                _logger.LogInformation($"User with Id: {userId} has already been confirmed");
                return PostResult.Success;
            }
            user.Confirm();
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return PostResult.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "User was not confirmed.");
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
                _logger.LogError(ex, $"Failed to ban user with Id: {userId}");
                return PostResult.UpdateFailed;
            }
        }
    }
}
