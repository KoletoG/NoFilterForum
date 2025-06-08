using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.OutputDTOs;
using Web.ViewModels.Reply;

namespace Web.Mappers
{
    public static class ReplyMapper
    {
        public static DeleteReplyRequest MapToRequest(DeleteReplyViewModel vm, string userId) => new()
        {
            ReplyId = vm.ReplyId,
            UserId = userId
        };
        public static CreateReplyRequest MapToRequest(CreateReplyViewModel vm, string userId) => new()
        {
            UserId = userId,
            Content = vm.Content,
            PostId = vm.PostId
        };
        public static GetListReplyIndexItemRequest MapToRequest(int page, string postId) => new()
        {
            Page = page,
            PostId = postId
        };
        public static GetReplyItemRequest MapToRequest(string username) => new()
        {
            Username = username
        };
        public static ReplyIndexItemViewModel MapToViewModel(ReplyIndexItemDto dto) => new()
        {
            Content = dto.Content,
            Id = dto.Id,
            DateCreated = dto.DateCreated,
            ImageUrl = dto.ImageUrl,
            Likes = dto.Likes,
            Role = dto.Role,
            UserId = dto.UserId,
            Username = dto.Username
        };
        public static PostReplyIndexViewModel MapToViewModel(PostReplyIndexDto dto) => new()
        {
            Id = dto.Id,
            Content = dto.Content,
            DateCreated = dto.DateCreated,
            ImageUrl = dto.ImageUrl,
            Likes = dto.Likes,
            Role = dto.Role,
            UserId = dto.UserId,
            Username = dto.Username
        };
    }
}
