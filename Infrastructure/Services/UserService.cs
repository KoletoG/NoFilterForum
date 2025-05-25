using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Global_variables;

namespace NoFilterForum.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _memoryCache;
        public UserService(IUserRepository userRepository, IMemoryCache memoryCache)
        {
            _userRepository = userRepository;
            _memoryCache = memoryCache;
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
            if (user==null)
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
    }
}
