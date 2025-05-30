using Core.Models.DTOs.InputDTOs;
using Web.ViewModels.Reply;

namespace Web.Mappers.Reply
{
    public static class ReplyMapper
    {
        public static DeleteReplyRequest MapToRequest(DeleteReplyViewModel vm, string userId) => new()
        {
            ReplyId = vm.ReplyId,
            UserId = userId
        };
    }
}
