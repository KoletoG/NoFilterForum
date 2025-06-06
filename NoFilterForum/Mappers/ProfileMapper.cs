using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.OutputDTOs;
using Microsoft.Extensions.Configuration.UserSecrets;
using Web.ViewModels.Post;
using Web.ViewModels.Profile;
using Web.ViewModels.Reply;

namespace Web.Mappers
{
    public class ProfileMapper
    {
        public static ChangeBioRequest MapToRequest(ChangeBioViewModel vm, string userId) => new()
        {
            Bio = vm.Bio,
            UserId = userId
        };
        public static UpdateImageRequest MapToRequest(UpdateImageViewModel vm, string userId) => new()
        {
            Image = vm.Image,
            UserId = userId
        };
        public static GetProfileDtoRequest MapToRequest(string username, string currentUsername) => new()
        {
            Username = username,
            CurrentUsername = currentUsername
        };
        public static ReplyItemViewModel MapToViewModel(ReplyItemDto dto) => new()
        {
            Content = dto.Content,
            Id = dto.Id,
            PostId = dto.PostId,
            Created = dto.Created
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
            Email = dto.Email,
            Id = dto.Id,
            ImageUrl = dto.ImageUrl,
            PostsCount = dto.PostsCount,
            Role = dto.Role,
            UserName = dto.UserName,
            WarningsCount = dto.WarningsCount,
        };
    }
}
