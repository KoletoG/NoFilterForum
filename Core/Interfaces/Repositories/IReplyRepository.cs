using Core.Models.DTOs.InputDTOs.Reply;
using Core.Models.DTOs.OutputDTOs.Reply;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface IReplyRepository
    {
        public Task<ReplyDataModel?> GetByIdAsync(string id);
        public Task<ReplyDataModel?> GetWithUserByIdAsync(string id, CancellationToken cancellationToken);
        public Task<string?> GetPostIdByIdAsync(string id, CancellationToken cancellationToken);
        public Task<IList<string>> GetIdsByPostIdAsync(string postId, CancellationToken cancellationToken);
        public Task<IReadOnlyCollection<ReplyIndexItemDto>> GetReplyIndexItemDtoListByPostIdAndPageAsync(GetListReplyIndexItemRequest getListReplyIndexItemRequest, CancellationToken cancellationToken);
        public Task<IDictionary<string, ReplyItemDto>> GetListReplyItemDtoByUserIdAsync(string userId, CancellationToken cancellationToken);
        public Task<IReadOnlyCollection<ReplyDataModel>> GetAllWithUserByPostIdAsync(string postId, CancellationToken cancellationToken);
        public Task<IReadOnlyCollection<ReplyDataModel>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken);
        public Task<DateTime> GetLastReplyDateTimeByUserIdAsync(string userId, CancellationToken cancellationToken);
        public Task CreateAsync(ReplyDataModel reply, CancellationToken cancellationToken);
        public void Update(ReplyDataModel reply);
        public void UpdateRange(IReadOnlyCollection<ReplyDataModel> replies);
        public void Delete(ReplyDataModel reply);
        public Task<int> GetCountByPostIdAsync(string postId, CancellationToken cancellationToken);
        public void DeleteRange(IReadOnlyCollection<ReplyDataModel> replies);
        public Task<bool> ExistByIdAsync(string id, CancellationToken cancellationToken);
    }
}
