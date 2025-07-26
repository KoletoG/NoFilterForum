using Core.Models.DTOs.InputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Reply;
using Microsoft.Extensions.Configuration.UserSecrets;
using Mono.TextTemplating;
using Web.ViewModels.Post;
using Web.ViewModels.Profile;
using Web.ViewModels.Reply;

namespace Web.Mappers
{
    public class ProfileMapper
    {
        public static ChangeBioRequest MapToRequest(ChangeBioViewModel vm, string userId) => new(vm.Bio, userId);
        public static UpdateImageRequest MapToRequest(UpdateImageViewModel vm, string userId) => new(userId, vm.Image);
        public static GetProfileDtoRequest MapToRequest(string userId, string currentUserId) => new(userId, currentUserId);
        public static ReplyItemViewModel MapToViewModel(ReplyItemDto dto) => new()
        {
            Content = dto.Content,
            Id = dto.Id,
            PostId = dto.PostId,
            Created = dto.Created,
            PostTitle = dto.PostTitle
        };
        public static PostItemViewModel MapToViewModel(ProfilePostDto dto) => new()
        {
            Id = dto.Id,
            Title = dto.Title,
            Created = dto.Created
        };
        public static ProfileViewModel MapToViewModel(List<PostItemViewModel> posts, List<ReplyItemViewModel> replies, bool isSameUser, ProfileUserViewModel profileUserDto, int page, Dictionary<string, DateTime> UserIdDate, int totalPages) => new()
        {
            IsSameUser = isSameUser,
            Posts = posts,
            Replies = replies,
            TotalPages = totalPages,
            Page = page,
            UserIdDate = UserIdDate,
            Profile = profileUserDto
        };
        public static ProfileUserViewModel MapToViewModel(ProfileUserDto dto) => new()
        {
            Bio = dto.Bio,
            DateCreated = dto.DateCreated,
            Email = dto.Email ?? string.Empty,
            Id = dto.Id ?? string.Empty,
            ImageUrl = dto.ImageUrl ?? string.Empty,
            PostsCount = dto.PostsCount,
            Role = dto.Role,
            UserName = dto.Username ?? string.Empty,
            WarningsCount = dto.WarningsCount,
        };
        public static ChangeEmailRequest MapToRequest(ChangeEmailViewModel vm, string userId) => new(userId, vm.Email);
        public static ChangeUsernameRequest MapToRequest(ChangeUsernameViewModel vm, string userId) => new(userId, vm.Username);
    }
}
