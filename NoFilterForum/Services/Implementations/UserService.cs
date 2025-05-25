using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NoFilterForum.Global_variables;
using NoFilterForum.Models.DataModels;
using NoFilterForum.Repositories.Interfaces;
using NoFilterForum.Services.Interfaces;

namespace NoFilterForum.Services.Implementations
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
    }
}
