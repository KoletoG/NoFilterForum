using Core.Enums;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.OutputDTOs;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IReplyService
    {
        public Task<PostResult> DeleteReplyAsync(DeleteReplyRequest request);
        public Task<bool> HasTimeoutByUserIdAsync(string userId);
        public Task<List<ReplyItemDto>> GetListReplyItemDtoAsync(GetReplyItemRequest getReplyItemRequest);
        public Task<PostResult> CreateReplyAsync(CreateReplyRequest createReplyRequest);
    }
}
