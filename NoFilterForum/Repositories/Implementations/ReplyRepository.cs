using Microsoft.EntityFrameworkCore;
using NoFilterForum.Data;
using NoFilterForum.Models.DataModels;
using NoFilterForum.Repositories.Interfaces;

namespace NoFilterForum.Repositories.Implementations
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
        public async Task<List<ReplyDataModel>> GetAllByUserAsync(UserDataModel user)
        {
            return await _context.ReplyDataModels.Where(x => x.User == user).ToListAsync();
        }
        public async Task<List<ReplyDataModel>> GetAllAsync()
        {
            return await _context.ReplyDataModels.ToListAsync();
        }
        public async Task<ReplyDataModel> CreateAsync(ReplyDataModel reply)
        {
            await _context.ReplyDataModels.AddAsync(reply);
            await _context.SaveChangesAsync();
            return reply;
        }
        public async Task UpdateAsync(ReplyDataModel reply)
        {
            _context.ReplyDataModels.Update(reply);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(ReplyDataModel reply)
        {
            _context.ReplyDataModels.Remove(reply);
            await _context.SaveChangesAsync();
        }
    }
}
