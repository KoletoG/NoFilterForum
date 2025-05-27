using Microsoft.EntityFrameworkCore;
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
        public async Task<ReplyDataModel> GetByIdAsync(string id)
        {
            return await _context.ReplyDataModels.FindAsync(id);
        }
        public async Task<List<ReplyDataModel>> GetAllByPostAsync(PostDataModel post)
        {
            return await _context.ReplyDataModels.Where(x => x.Post == post).ToListAsync();
        }
        public async Task<List<ReplyDataModel>> GetAllByUserIdAsync(string userId)
        {
            return await _context.ReplyDataModels.Where(x => x.User.Id == userId).ToListAsync() ?? new List<ReplyDataModel>();
        }
        public async Task<List<ReplyDataModel>> GetAllAsync()
        {
            return await _context.ReplyDataModels.ToListAsync();
        }
        public async Task<ReplyDataModel> CreateAsync(ReplyDataModel reply)
        {
            await _context.ReplyDataModels.AddAsync(reply);
            return reply;
        }
        public async Task<int> GetCountByPostIdAsync(string postId)
        {
            return await _context.ReplyDataModels.Where(x=>x.Post.Id==postId).CountAsync();
        }
        public async Task<bool> UpdateAsync(ReplyDataModel reply)
        {
            _context.ReplyDataModels.Update(reply);
            return true;
        }
        public async Task<bool> UpdateRangeAsync(List<ReplyDataModel> replies)
        {
            _context.ReplyDataModels.UpdateRange(replies);
            return true;
        }
        public async Task DeleteAsync(ReplyDataModel reply)
        {
            _context.ReplyDataModels.Remove(reply);
        }
        public async Task DeleteRangeAsync(List<ReplyDataModel> replies)
        {
            _context.ReplyDataModels.RemoveRange(replies);
        }
    }
}
