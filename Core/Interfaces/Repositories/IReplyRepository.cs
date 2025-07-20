using Core.Models.DTOs.OutputDTOs.Reply;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface IReplyRepository
    {
        public Task<ReplyDataModel?> GetByIdAsync(string id);
        public Task<ReplyDataModel?> GetWithUserByIdAsync(string id);
        public Task<string?> GetPostIdById(string id);
        public Task<List<string>> GetIdsByPostIdAsync(string postId);
        public Task<List<ReplyIndexItemDto>> GetReplyIndexItemDtoListByPostIdAndPageAsync(string postId, int page, int repliesPerPage);
        public Task<List<ReplyItemDto>> GetListReplyItemDtoByUserIdAsync(string userId);
        public Task<UserDataModel?> GetUserByReplyIdAsync(string replyId);
        public Task<List<ReplyDataModel>> GetAllWithUserByPostIdAsync(string postId);
        public Task<List<ReplyDataModel>> GetAllByPostIdAsync(string postId);
        public Task<List<ReplyDataModel>> GetAllByUserIdAsync(string userId);
        public Task<DateTime> GetLastReplyDateTimeByUserIdAsync(string userId);
        public Task<List<ReplyDataModel>> GetAllAsync();
        public Task<ReplyDataModel> CreateAsync(ReplyDataModel reply);
        public void Update(ReplyDataModel reply);
        public Task<string?> GetUserIdByReplyIdAsync(string replyId);
        public void UpdateRange(List<ReplyDataModel> replies);
        public void Delete(ReplyDataModel reply);
        public Task<int> GetCountByPostIdAsync(string postId);
        public void DeleteRange(List<ReplyDataModel> replies);
        public Task<bool> ExistByIdAsync(string id);
    }
}
