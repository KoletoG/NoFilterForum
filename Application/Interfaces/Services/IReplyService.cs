using Core.DTOs.InputDTOs.Reply;
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
        public Task<PostResult> DeleteReplyAsync(DeleteReplyRequest request, CancellationToken cancellationToken);
        public Task<bool> HasTimeoutByUserIdAsync(string userId, CancellationToken cancellationToken);
        public Task<PostResult> LikeAsync(LikeDislikeRequest likeDislikeRequest, CancellationToken cancellationToken);
        public Task<PostResult> DislikeAsync(LikeDislikeRequest likeDislikeRequest, CancellationToken cancellationToken);
        public Task<PageTotalPagesDTO> GetPageTotalPagesDTOByReplyIdAndPostIdAsync(string replyId, string postId, CancellationToken cancellationToken);
        public Task<PageTotalPagesDTO> GetPageAndTotalPagesDTOByPostIdAsync(string postId, int page, CancellationToken cancellationToken);
        public Task<IReadOnlyCollection<ReplyIndexItemDto>> GetListReplyIndexItemDto(GetListReplyIndexItemRequest getListReplyIndexItemRequest, CancellationToken cancellationToken);
        public Task<IReadOnlyCollection<ReplyItemDto>> GetListReplyItemDtoAsync(string userId, CancellationToken cancellationToken);
        public Task<PostResult> CreateReplyAsync(CreateReplyRequest createReplyRequest, CancellationToken cancellationToken);
    }
}
