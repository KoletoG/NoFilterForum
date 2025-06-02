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
    }
}
