using Core.Models.DTOs.InputDTOs.Reply;
using Core.Models.DTOs.OutputDTOs.Reply;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Infrastructure.Data;

namespace NoFilterForum.Infrastructure.Repositories
{
    public class ReplyRepository : IReplyRepository
    {
        private readonly ApplicationDbContext _context;

        public ReplyRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ReplyDataModel?> GetByIdAsync(string id)
        {
            return await _context.ReplyDataModels.FindAsync(id);
        }
        public async Task<IList<string>> GetIdsByPostIdAsync(string postId)
        {
            return await _context.ReplyDataModels.Where(x=>x.Post.Id== postId).OrderBy(x=>x.DateCreated).Select(x=>x.Id).ToListAsync();
        }
        public async Task<IReadOnlyCollection<ReplyIndexItemDto>> GetReplyIndexItemDtoListByPostIdAndPageAsync(GetListReplyIndexItemRequest getListReplyIndexItemRequest, CancellationToken cancellationToken)
        {
            return await _context.ReplyDataModels.AsNoTracking()
                .Where(x => x.Post.Id == getListReplyIndexItemRequest.PostId)
                .OrderBy(x=>x.DateCreated)
                .Select(x => new ReplyIndexItemDto(x.User.UserName,x.User.Role,x.User.ImageUrl,x.User.Id,x.Id,x.Content,x.Likes,x.DateCreated))
                .Skip((getListReplyIndexItemRequest.Page-1)*getListReplyIndexItemRequest.PostsCount)
                .Take(getListReplyIndexItemRequest.PostsCount)
                .ToListAsync(cancellationToken);
        }
        public async Task<IReadOnlyCollection<ReplyItemDto>> GetListReplyItemDtoByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _context.ReplyDataModels.AsNoTracking()
                .Where(x=>x.User.Id == userId)
                .Select(x => new ReplyItemDto(x.Id,x.Post.Id,x.Content,x.DateCreated,x.Post.Title))
                .ToListAsync(cancellationToken);
        }
        public async Task<ReplyDataModel?> GetWithUserByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _context.ReplyDataModels.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id,cancellationToken);
        }
        public async Task<string?> GetPostIdByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _context.ReplyDataModels.Where(x => x.Id == id).Select(x => x.Post.Id).FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<IReadOnlyCollection<ReplyDataModel>> GetAllWithUserByPostIdAsync(string postId, CancellationToken cancellationToken)
        {
            return await _context.ReplyDataModels.Include(x => x.User).Where(x=>x.Post.Id==postId).ToListAsync(cancellationToken);
        }
        public async Task<UserDataModel?> GetUserByReplyIdAsync(string replyId)
        {
            return await _context.ReplyDataModels.Where(x => x.Id == replyId).Select(x => x.User).FirstOrDefaultAsync();
        }
        public async Task<IReadOnlyCollection<ReplyDataModel>> GetAllByPostIdAsync(string postId)
        {
            return await _context.ReplyDataModels.Where(x => x.Post.Id == postId).ToListAsync();
        }
        public async Task<IReadOnlyCollection<ReplyDataModel>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _context.ReplyDataModels.Where(x => x.User.Id == userId).ToListAsync(cancellationToken);
        }
        public async Task<string?> GetUserIdByReplyIdAsync(string replyId)
        {
            return await _context.ReplyDataModels.Where(x=>x.Id==replyId).Select(x=>x.User.Id).FirstOrDefaultAsync();
        }
        public async Task<IReadOnlyCollection<ReplyDataModel>> GetAllAsync()
        {
            return await _context.ReplyDataModels.ToListAsync();
        }
        public async Task CreateAsync(ReplyDataModel reply, CancellationToken cancellationToken)
        {
            await _context.ReplyDataModels.AddAsync(reply,cancellationToken);
        }
        public async Task<int> GetCountByPostIdAsync(string postId)
        {
            return await _context.ReplyDataModels.Where(x=>x.Post.Id==postId).CountAsync();
        }
        public void Update(ReplyDataModel reply)
        {
            _context.ReplyDataModels.Update(reply);
        }
        public async Task<DateTime> GetLastReplyDateTimeByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _context.ReplyDataModels.Where(x => x.User.Id == userId).Select(x => x.DateCreated).OrderByDescending(x => x.Date).FirstOrDefaultAsync(cancellationToken);
        }
        public void UpdateRange(IReadOnlyCollection<ReplyDataModel> replies)
        {
            _context.ReplyDataModels.UpdateRange(replies);
        }
        public void Delete(ReplyDataModel reply)
        {
            _context.ReplyDataModels.Remove(reply);
        }
        public async Task<bool> ExistByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _context.ReplyDataModels.AnyAsync(x => x.Id == id, cancellationToken);
        }
        public void DeleteRange(IReadOnlyCollection<ReplyDataModel> replies)
        {
            _context.ReplyDataModels.RemoveRange(replies);
        }
    }
}
