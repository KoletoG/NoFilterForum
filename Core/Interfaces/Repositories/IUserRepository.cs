using Core.Enums;
using Core.Models.DTOs.OutputDTOs.Admin;
using Core.Models.DTOs.OutputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Reply;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public IQueryable<UserDataModel> GetAll();
        public Task<bool> ExistByIdAsync(string id, CancellationToken cancellationToken);
        public Task<UserDataModel?> GetByIdAsync(string id);
        public Task<ProfileUserDto?> GetProfileUserDtoByIdAsync(string id, CancellationToken cancellationToken);
        public Task<CurrentUserReplyIndexDto?> GetCurrentUserReplyIndexDtoByIdAsync(string id, CancellationToken cancellationToken);
        public Task<UserDataModel?> GetUserWithWarningsByIdAsync(string id, CancellationToken cancellationToken);
        public Task<IReadOnlyCollection<UserForAdminPanelDto>> GetUserItemsForAdminDtoAsync(CancellationToken cancellationToken);
        public Task<IReadOnlyCollection<UserDataModel>> GetListByUsernameArrayAsync(string[] usernames, CancellationToken cancellationToken);
        public void UpdateRange(IEnumerable<UserDataModel> users);
        public Task<bool> ExistNormalizedUsername(string normalizedUsername, CancellationToken cancellationToken);
        public Task<bool> ExistNormalizedEmailAsync(string normalizedEmail, CancellationToken cancellationToken);
        public Task<bool> ExistsByNotConfirmedAsync(CancellationToken cancellationToken);
        public void Update(UserDataModel user);
        public void Delete(UserDataModel user);
        public Task<bool> ExistsUsernameAsync(string username, CancellationToken cancellationToken);
        public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken);
        public Task<bool> ExistsByUsernameAsync(string username);
        public Task<IReadOnlyCollection<UsersReasonsDto>> GetAllUnconfirmedUserDtosAsync(CancellationToken cancellationToken);
    }
}
