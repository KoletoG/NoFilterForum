using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface IReplyRepository
    {
        public Task<ReplyDataModel> GetByIdAsync(string id);
        public Task<List<ReplyDataModel>> GetAllByPostIdAsync(string postId);
        public Task<List<ReplyDataModel>> GetAllByUserIdAsync(string userId);
        public Task<List<ReplyDataModel>> GetAllAsync();
        public Task<ReplyDataModel> CreateAsync(ReplyDataModel reply);
        public Task<bool> UpdateAsync(ReplyDataModel reply);
        public Task<string> GetUserIdByReplyIdAsync(string replyId);
        public Task<bool> UpdateRangeAsync(List<ReplyDataModel> replies);
        public Task DeleteAsync(ReplyDataModel reply);
        public Task<int> GetCountByPostIdAsync(string postId);
        public Task DeleteRangeAsync(List<ReplyDataModel> replies);
    }
}
