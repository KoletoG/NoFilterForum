using Core.Models.DTOs.InputDTOs.Reply;
using Core.Models.DTOs.OutputDTOs.Reply;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface IReplyRepository
    {
        public Task<ReplyDataModel?> GetByIdAsync(string id);
        public Task<ReplyDataModel?> GetWithUserByIdAsync(string id);
        public Task<string?> GetPostIdByIdAsync(string id, CancellationToken cancellationToken);
        public Task<IList<string>> GetIdsByPostIdAsync(string postId);
        public Task<IReadOnlyCollection<ReplyIndexItemDto>> GetReplyIndexItemDtoListByPostIdAndPageAsync(GetListReplyIndexItemRequest getListReplyIndexItemRequest);
        public Task<IReadOnlyCollection<ReplyItemDto>> GetListReplyItemDtoByUserIdAsync(string userId);
        public Task<UserDataModel?> GetUserByReplyIdAsync(string replyId);
        public Task<IReadOnlyCollection<ReplyDataModel>> GetAllWithUserByPostIdAsync(string postId);
        public Task<IReadOnlyCollection<ReplyDataModel>> GetAllByPostIdAsync(string postId);
        public Task<IReadOnlyCollection<ReplyDataModel>> GetAllByUserIdAsync(string userId);
        public Task<DateTime> GetLastReplyDateTimeByUserIdAsync(string userId);
        public Task<IReadOnlyCollection<ReplyDataModel>> GetAllAsync();
        public Task<ReplyDataModel> CreateAsync(ReplyDataModel reply);
        public void Update(ReplyDataModel reply);
        public Task<string?> GetUserIdByReplyIdAsync(string replyId);
        public void UpdateRange(IReadOnlyCollection<ReplyDataModel> replies);
        public void Delete(ReplyDataModel reply);
        public Task<int> GetCountByPostIdAsync(string postId);
        public void DeleteRange(IReadOnlyCollection<ReplyDataModel> replies);
        public Task<bool> ExistByIdAsync(string id, CancellationToken cancellationToken);
    }
}
