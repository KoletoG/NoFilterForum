using System.Web;
using Core.Constants;
using Core.DTOs.OutputDTOs.Reply;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.InputDTOs.Post;
using Core.Models.DTOs.InputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Post;
using Core.Models.DTOs.OutputDTOs.Profile;
using Web.ViewModels.Post;

namespace Web.Mappers
{
    public static class PostMapper
    {
        public static PostItemViewModel MapToViewModel(ProfilePostDto dto) => new()
        {
            Id = dto.Id,
            Created = dto.Created,
            Title = dto.Title
        };
        public static CreatePostRequest MapToRequest(CreatePostViewModel vm, string userId) => new(vm.Title, vm.Body, vm.TitleOfSection, userId);
        public static PostIndexViewModel MapToViewModel(IEnumerable<PostIndexItemViewModel> itemVMs, PageTotalPagesDTO pageTotalPages, string titleOfSection, bool isAdmin) => new()
        {
            Posts = itemVMs,
            Page = pageTotalPages.Page,
            TotalPages = pageTotalPages.TotalPages,
            TitleOfSection = HttpUtility.UrlEncode(titleOfSection),
            IsAdmin = isAdmin
        };
        public static PostIndexItemViewModel MapToViewModel(PostItemDto dto) => new(dto.PostLikes)
        {
            DateCreated = dto.DateCreated,
            Id= dto.Id,
            IsPinned= dto.IsPinned,
            Title = dto.Title,
            Role= dto.Role,
            Username= dto.Username,
            UserImageUrl = dto.ImageUrl,
            Body = dto.Body
        };
        public static GetIndexPostRequest MapToRequest(int page, string titleOfSection) => new(titleOfSection, page,PostConstants.PostsPerSection);
        public static DeletePostRequest MapToRequest(DeletePostViewModel vm, string userId) => new(vm.PostId, userId);
        public static LikeDislikeRequest MapToRequest(string postId, string userId) => new()
        {
            PostReplyId = postId,
            UserId = userId
        };
    }
}
