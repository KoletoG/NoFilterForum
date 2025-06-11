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
        public static CreatePostRequest MapToRequest(CreatePostViewModel vm, string userId) => new()
        {
            Body = vm.Body,
            Title = vm.Title,
            TitleOfSection = vm.TitleOfSection,
            UserId = userId
        };
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
            Username= dto.Username
        };
        public static GetIndexPostRequest MapToRequest(int page, string titleOfSection) => new()
        {
            Page = page,
            TitleOfSection = titleOfSection
        };
        public static DeletePostRequest MapToRequest(DeletePostViewModel vm, string userId) => new()
        {
            PostId = vm.PostId,
            UserId = userId
        };
        public static GetProfilePostDtoRequest MapToRequest(string username) => new()
        {
            Username = username
        };
        public static LikeDislikeRequest MapToRequest(string postId, string userId) => new()
        {
            PostReplyId = postId,
            UserId = userId
        };
    }
}
