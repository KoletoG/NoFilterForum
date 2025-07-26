using System.Web;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.InputDTOs.Post;
using Core.Models.DTOs.InputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Post;
using Web.ViewModels.Post;

namespace Web.Mappers
{
    public static class PostMappers
    {
        public static CreatePostRequest MapToRequest(CreatePostViewModel vm, string userId) => new(vm.Title, vm.Body, vm.TitleOfSection, userId);
        public static PostIndexViewModel MapToViewModel(List<PostIndexItemViewModel> itemVMs, int page, int totalPages, string titleOfSection) => new()
        {
            Posts = itemVMs,
            Page = page,
            TotalPages = totalPages,
            TitleOfSection = HttpUtility.UrlEncode(titleOfSection)
        };
        public static PostIndexItemViewModel MapToViewModel(PostItemDto dto) => new()
        {
            DateCreated = dto.DateCreated,
            Id= dto.Id,
            IsPinned= dto.IsPinned,
            Title = dto.Title,
            Role= dto.Role,
            Username= dto.Username,
            UserImageUrl = dto.ImageUrl,
            PostLikes = dto.PostLikes
        };
        public static GetIndexPostRequest MapToRequest(int page, string titleOfSection) => new(titleOfSection, page);
        public static DeletePostRequest MapToRequest(DeletePostViewModel vm, string userId) => new(vm.PostId, userId);
        public static GetProfilePostDtoRequest MapToRequest(string userId) => new(userId);
        public static LikeDislikeRequest MapToRequest(string postId, string userId) => new()
        {
            PostReplyId = postId,
            UserId = userId
        };
    }
}
