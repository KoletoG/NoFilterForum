using Core.Enums;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.OutputDTOs;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IUserService
    {
        public bool IsDefaultUsername(string username);
        public Dictionary<string, DateTime> OrderDates(List<ProfilePostDto> postItemDtos, List<ReplyItemDto> replyItemDtos, int page, int countPerPage);
        public Task<bool> AnyNotConfirmedUsersAsync();
        public Task<ProfileDto> GetProfileDtoByUsernameAsync(GetProfileDtoRequest getProfileDtoRequest);
        public Task<PostResult> ChangeBioAsync(ChangeBioRequest changeBioRequest);
        public Task<PostResult> UpdateImageAsync(UpdateImageRequest updateImageRequest);
        public Task<List<UserForAdminPanelDto>> GetAllUsersWithoutDefaultAsync();
        public Task<List<UserDataModel>> GetAllUnconfirmedUsersAsync();
        public Task<PostResult> ChangeUsernameByIdAsync(ChangeUsernameRequest changeUsernameRequest);
        public Task<PostResult> ConfirmUserAsync(string userId);
        public Task<PostResult> BanUserByIdAsync(string userId);
        public Task<UserDataModel> GetUserByIdAsync(string id);
        public Task<bool> UsernameExistsAsync(string username);
        public int GetTotalCountByPostsAndReplies(List<ReplyItemDto> replies, List<ProfilePostDto> posts);
        public Task<bool> EmailExistsAsync(string email);
        public Task<bool> IsAdminRoleByIdAsync(string userId);
        public Task<PostResult> ChangeEmailByIdAsync(ChangeEmailRequest changeEmailRequest);
    }
}
