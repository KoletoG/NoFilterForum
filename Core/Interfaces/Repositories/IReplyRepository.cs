using Core.Models.DTOs.OutputDTOs.Reply;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface IReplyRepository
    {
        public Task<ReplyDataModel> GetByIdAsync(string id);
        public Task<ReplyDataModel> GetWithUserByIdAsync(string id);
        public Task<string> GetPostIdById(string id);
        public Task<List<string>> GetIdsByPostIdAsync(string postId);
        public Task<List<ReplyIndexItemDto>> GetReplyIndexItemDtoListByPostIdAndPageAsync(string postId, int page, int repliesPerPage);
        public Task<List<ReplyItemDto>> GetListReplyItemDtoByUsernameAsync(string username);
        public Task<UserDataModel> GetUserByReplyIdAsync(string replyId);
        public Task<List<ReplyDataModel>> GetAllWithUserByPostIdAsync(string postId);
        public Task<List<ReplyDataModel>> GetAllByPostIdAsync(string postId);
        public Task<List<ReplyDataModel>> GetAllByUserIdAsync(string userId);
        public Task<DateTime> GetLastReplyDateTimeByUserIdAsync(string userId);
        public Task<List<ReplyDataModel>> GetAllAsync();
        public Task<ReplyDataModel> CreateAsync(ReplyDataModel reply);
        public Task<bool> UpdateAsync(ReplyDataModel reply);
        public Task<string> GetUserIdByReplyIdAsync(string replyId);
        public Task<bool> UpdateRangeAsync(List<ReplyDataModel> replies);
        public Task DeleteAsync(ReplyDataModel reply);
        public Task<int> GetCountByPostIdAsync(string postId);
        public Task DeleteRangeAsync(List<ReplyDataModel> replies);
        public Task<bool> ExistByIdAsync(string id);
    }
}
