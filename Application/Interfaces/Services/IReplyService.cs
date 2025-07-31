using Core.DTOs.OutputDTOs.Reply;
using Core.Enums;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.InputDTOs.Reply;
using Core.Models.DTOs.OutputDTOs.Reply;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IReplyService
    {
        public Task<PostResult> DeleteReplyAsync(DeleteReplyRequest request);
        public Task<bool> HasTimeoutByUserIdAsync(string userId);
        public Task<PostResult> LikeAsync(LikeDislikeRequest likeDislikeRequest);
        public Task<PostResult> DislikeAsync(LikeDislikeRequest likeDislikeRequest);
        public Task<PageTotalPagesDTO> GetPageTotalPagesDTOByReplyIdAndPostIdAsync(string replyId, string postId);
        public Task<PageTotalPagesDTO> GetPageAndTotalPagesDTOByPostIdAsync(string postId, int page);
        public Task<List<ReplyIndexItemDto>> GetListReplyIndexItemDto(GetListReplyIndexItemRequest getListReplyIndexItemRequest);
        public Task<List<ReplyItemDto>> GetListReplyItemDtoAsync(string userId);
        public Task<PostResult> CreateReplyAsync(CreateReplyRequest createReplyRequest);
    }
}
