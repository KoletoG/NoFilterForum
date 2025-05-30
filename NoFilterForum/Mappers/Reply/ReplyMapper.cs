using Core.Models.DTOs.InputDTOs;
using Web.ViewModels.Reply;

namespace Web.Mappers.Reply
{
    public static class ReplyMapper
    {
        public static DeleteReplyRequest MapToRequest(DeleteReplyViewModel vm) => new()
        {
            ReplyId = vm.ReplyId
        };
    }
}
