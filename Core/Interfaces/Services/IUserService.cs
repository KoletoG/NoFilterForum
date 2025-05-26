using Core.Enums;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IUserService
    {
        public Task<List<UserDataModel>> GetAllUsersWithoutDefaultAsync();
        public Task<bool> AnyNotConfirmedUsersAsync();
        public Task<List<UserDataModel>> GetAllUnconfirmedUsersAsync();
        public Task<PostResult> ConfirmUserAsync(string userId);
    }
}
