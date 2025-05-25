using NoFilterForum.Repositories.Interfaces;
using NoFilterForum.Services.Interfaces;

namespace NoFilterForum.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
    }
}
