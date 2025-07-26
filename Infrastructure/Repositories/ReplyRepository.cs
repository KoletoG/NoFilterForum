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
        public async Task<List<string>> GetIdsByPostIdAsync(string postId)
        {
            return await _context.ReplyDataModels.Where(x=>x.Post.Id== postId).Select(x=>x.Id).ToListAsync();
        }
        public async Task<List<ReplyIndexItemDto>> GetReplyIndexItemDtoListByPostIdAndPageAsync(string postId, int page, int repliesPerPage)
        {
            return await _context.ReplyDataModels.Where(x => x.Post.Id == postId).OrderBy(x=>x.DateCreated).Select(x => new ReplyIndexItemDto(x.User.UserName,x.User.Role,x.User.ImageUrl,x.User.Id,x.Id,x.Content,x.Likes,x.DateCreated)).Skip((page-1)*repliesPerPage).Take(repliesPerPage).ToListAsync();
        }
        public async Task<List<ReplyItemDto>> GetListReplyItemDtoByUserIdAsync(string userId)
        {
            return await _context.ReplyDataModels.Where(x=>x.User.Id == userId).Select(x => new ReplyItemDto(x.Id,x.Post.Id,x.Content,x.DateCreated,x.Post.Title)).ToListAsync();
        }
        public async Task<ReplyDataModel?> GetWithUserByIdAsync(string id)
        {
            return await _context.ReplyDataModels.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<string?> GetPostIdById(string id)
        {
            return await _context.ReplyDataModels.Where(x => x.Id == id).Select(x => x.Post.Id).FirstOrDefaultAsync();
        }
        public async Task<List<ReplyDataModel>> GetAllWithUserByPostIdAsync(string postId)
        {
            return await _context.ReplyDataModels.Include(x => x.User).Where(x=>x.Post.Id==postId).ToListAsync();
        }
        public async Task<UserDataModel?> GetUserByReplyIdAsync(string replyId)
        {
            return await _context.ReplyDataModels.Where(x => x.Id == replyId).Select(x => x.User).FirstOrDefaultAsync();
        }
        public async Task<List<ReplyDataModel>> GetAllByPostIdAsync(string postId)
        {
            return await _context.ReplyDataModels.Where(x => x.Post.Id == postId).ToListAsync();
        }
        public async Task<List<ReplyDataModel>> GetAllByUserIdAsync(string userId)
        {
            return await _context.ReplyDataModels.Where(x => x.User.Id == userId).ToListAsync() ?? new List<ReplyDataModel>();
        }
        public async Task<string?> GetUserIdByReplyIdAsync(string replyId)
        {
            return await _context.ReplyDataModels.Where(x=>x.Id==replyId).Select(x=>x.User.Id).FirstOrDefaultAsync();
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
        public void Update(ReplyDataModel reply)
        {
            _context.ReplyDataModels.Update(reply);
        }
        public async Task<DateTime> GetLastReplyDateTimeByUserIdAsync(string userId)
        {
            return await _context.ReplyDataModels.Where(x => x.User.Id == userId).Select(x => x.DateCreated).OrderByDescending(x => x.Date).FirstOrDefaultAsync();
        }
        public void UpdateRange(List<ReplyDataModel> replies)
        {
            _context.ReplyDataModels.UpdateRange(replies);
        }
        public void Delete(ReplyDataModel reply)
        {
            _context.ReplyDataModels.Remove(reply);
        }
        public async Task<bool> ExistByIdAsync(string id)
        {
            return await _context.ReplyDataModels.AnyAsync(x => x.Id == id);
        }
        public void DeleteRange(List<ReplyDataModel> replies)
        {
            _context.ReplyDataModels.RemoveRange(replies);
        }
    }
}
