using Core.Enums;
using Core.Models.DTOs.OutputDTOs.Admin;
using Core.Models.DTOs.OutputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Reply;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<UserDataModel?> GetByIdAsync(string id);
        public Task<UserRoles> GetUserRoleIdAsync(string userId);
        public Task<UserDataModel?> GetByUsernameAsync(string username);
        public Task<ProfileUserDto?> GetProfileUserDtoByIdAsync(string id);
        public Task<CurrentUserReplyIndexDto?> GetCurrentUserReplyIndexDtoByIdAsync(string id);
        public Task<UserDataModel?> GetUserWithWarningsByIdAsync(string id);
        public Task<List<UserForAdminPanelDto>> GetUserItemsForAdminDtoAsync();
        public Task<List<UserDataModel>> GetAllAsync();
        public Task<List<UserDataModel>> GetListByUsernameArrayAsync(string[] usernames);
        public void UpdateRange(List<UserDataModel> users);
        public Task<UserDataModel> CreateAsync(UserDataModel user);
        public Task<bool> ExistsByNotConfirmedAsync();
        public void Update(UserDataModel user);
        public void Delete(UserDataModel user);
        public Task<List<UserDataModel>> GetAllNoDefaultAsync();
        public Task<bool> UsernameExistsAsync(string username);
        public Task<bool> EmailExistsAsync(string email);
        public Task<bool> ExistsByUsernameAsync(string username);
        public Task<List<UsersReasonsDto>> GetAllUnconfirmedUserDtosAsync();
    }
}
