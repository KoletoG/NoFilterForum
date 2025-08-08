using Core.Enums;
using Core.Models.DTOs.InputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Admin;
using Core.Models.DTOs.OutputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Reply;
using NoFilterForum.Core.Models.DataModels;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        public bool IsDefaultUserId(string username);
        public Task<bool> AnyNotConfirmedUsersAsync(CancellationToken cancellationToken);
        public Task ApplyRoleAsync(UserDataModel user);
        public Task<bool> IsAdminOrVIPAsync(string userId);
        public Task<bool> IsAdminAsync(string userId);
        public Task<CurrentUserReplyIndexDto?> GetCurrentUserReplyIndexDtoByIdAsync(string id, CancellationToken cancellationToken);
        public Task<ProfileDto> GetProfileDtoByUserIdAsync(GetProfileDtoRequest getProfileDtoRequest);
        public Task<PostResult> ChangeBioAsync(ChangeBioRequest changeBioRequest);
        public Task<PostResult> UpdateImageAsync(UpdateImageRequest updateImageRequest);
        public Task<IEnumerable<UserForAdminPanelDto>> GetAllUsersWithoutDefaultAsync(CancellationToken cancellationToken);
        public Task<IReadOnlyCollection<UsersReasonsDto>> GetAllUnconfirmedUsersAsync(CancellationToken cancellationToken);
        public Task<PostResult> ChangeUsernameByIdAsync(ChangeUsernameRequest changeUsernameRequest, CancellationToken cancellationToken);
        public Task<PostResult> ConfirmUserAsync(string userId, CancellationToken cancellationToken);
        public Task<PostResult> BanUserByIdAsync(string userId, CancellationToken cancellationToken);
        public Task<UserDataModel?> GetUserByIdAsync(string id);
        public Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken);
        public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken);
        public Task<PostResult> ChangeEmailByIdAsync(ChangeEmailRequest changeEmailRequest, CancellationToken cancellationToken);
    }
}
