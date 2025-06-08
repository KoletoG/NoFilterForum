using Core.Models.DTOs.InputDTOs;
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
    }
}
