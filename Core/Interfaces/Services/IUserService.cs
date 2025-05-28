using Core.Enums;
using Core.Models.DTOs.InputDTOs;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IUserService
    {
        public Task<List<UserDataModel>> GetAllUsersWithoutDefaultAsync();
        public Task<bool> AnyNotConfirmedUsersAsync();
        public Task<List<UserDataModel>> GetAllUnconfirmedUsersAsync();
        public Task<PostResult> ChangeUsernameByIdAsync(ChangeUsernameRequest changeUsernameRequest);
        public Task<PostResult> ConfirmUserAsync(string userId);
        public Task<PostResult> BanUserByIdAsync(string userId);
        public Task<UserDataModel> GetUserWithWarningsByIdAsync(string userId);
        public Task<UserDataModel> GetUserByIdAsync(string id);
        public Task<bool> UsernameExistsAsync(string username);
        public Task<bool> EmailExistsAsync(string email);
        public Task<PostResult> ChangeEmailByIdAsync(ChangeEmailRequest changeEmailRequest);
    }
}
