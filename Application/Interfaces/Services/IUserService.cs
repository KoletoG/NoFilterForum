using Core.Enums;
using Core.Models.DTOs.InputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Admin;
using Core.Models.DTOs.OutputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Reply;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IUserService
    {
        public bool IsDefaultUserId(string username);
        public Task<bool> AnyNotConfirmedUsersAsync();
        public Task ApplyRoleAsync(UserDataModel user);
        public Task<bool> IsAdminOrVIPAsync(string userId);
        public Task<bool> IsAdminAsync(string userId);
        public Task<CurrentUserReplyIndexDto?> GetCurrentUserReplyIndexDtoByIdAsync(string id);
        public Task<ProfileDto> GetProfileDtoByUserIdAsync(GetProfileDtoRequest getProfileDtoRequest);
        public Task<PostResult> ChangeBioAsync(ChangeBioRequest changeBioRequest);
        public Task<PostResult> UpdateImageAsync(UpdateImageRequest updateImageRequest);
        public Task<IEnumerable<UserForAdminPanelDto>> GetAllUsersWithoutDefaultAsync();
        public Task<IReadOnlyCollection<UsersReasonsDto>> GetAllUnconfirmedUsersAsync();
        public Task<PostResult> ChangeUsernameByIdAsync(ChangeUsernameRequest changeUsernameRequest);
        public Task<PostResult> ConfirmUserAsync(string userId);
        public Task<PostResult> BanUserByIdAsync(string userId);
        public Task<UserDataModel?> GetUserByIdAsync(string id);
        public Task<bool> UsernameExistsAsync(string username);
        public Task<bool> EmailExistsAsync(string email);
        public Task<PostResult> ChangeEmailByIdAsync(ChangeEmailRequest changeEmailRequest);
    }
}
