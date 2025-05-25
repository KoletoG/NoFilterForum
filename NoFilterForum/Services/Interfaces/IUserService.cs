using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Services.Interfaces
{
    public interface IUserService
    {
        public Task<List<UserDataModel>> GetAllUsersWithoutDefaultAsync();
        public Task<bool> AnyNotConfirmedUsersAsync();
        public Task<List<UserDataModel>> GetAllUnconfirmedUsersAsync();
        public Task ConfirmUserAsync(string userId);
    }
}
