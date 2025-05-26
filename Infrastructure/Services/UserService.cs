using Core.Constants;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly IPostRepository _postRepository;
        private readonly IReplyRepository _replyRepository;
        public UserService(IUserRepository userRepository, IMemoryCache memoryCache, IPostRepository postRepository, IReplyRepository replyRepository)
        {
            _userRepository = userRepository;
            _memoryCache = memoryCache;
            _postRepository = postRepository;
            _replyRepository = replyRepository;
        }
        // Add paging
        public async Task<List<UserDataModel>> GetAllUsersWithoutDefaultAsync()
        {
            if (!_memoryCache.TryGetValue($"usersListNoDefault", out List<UserDataModel> users))
            {
                users = await _userRepository.GetAllNoDefaultAsync();
                _memoryCache.Set($"usersListNoDefault", users, TimeSpan.FromMinutes(5));
            }
            return users;
        }
        public async Task<bool> AnyNotConfirmedUsersAsync()
        {
            return await _userRepository.ExistsByNotConfirmedAsync();
        }
        public async Task<List<UserDataModel>> GetAllUnconfirmedUsersAsync()
        {
            return await _userRepository.GetAllUnconfirmedAsync();
        }
        public async Task ConfirmUserAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return; // Change these, HANDLE THE ERRORS BETTER
            }
            if (user.IsConfirmed)
            {
                return; // Change these, HANDLE THE ERRORS BETTER
            }
            user.Confirm();
            await _userRepository.UpdateAsync(user);
        }
        public async Task<PostResult> BanUserByIdAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return PostResult.NotFound;
            }
            var posts = await _postRepository.GetAllByUserIdAsync(userId);
            var replies = await _replyRepository.GetAllByUserIdAsync(userId);
            bool success = true;
            foreach (var post in posts)
            {
                post.SetDefaultUser();
            }
            success = await _postRepository.UpdateRangeAsync(posts);
            if (success)
            {
                foreach (var reply in replies)
                {
                    reply.SetDefaultUser();
                }
                success = await _replyRepository.UpdateRangeAsync(replies);
            }
            await _userRepository.DeleteAsync(user);
            return success switch
            {
                false => PostResult.UpdateFailed,
                true => PostResult.Success
            };
        }
    }
}
