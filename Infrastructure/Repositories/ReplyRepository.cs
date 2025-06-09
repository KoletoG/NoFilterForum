using Core.Models.DTOs.OutputDTOs;
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
        public async Task<ReplyDataModel> GetByIdAsync(string id)
        {
            return await _context.ReplyDataModels.FindAsync(id);
        }
        public async Task<List<string>> GetIdsByPostIdAsync(string postId)
        {
            return await _context.ReplyDataModels.Where(x=>x.Post.Id== postId).Select(x=>x.Id).ToListAsync();
        }
        public async Task<List<ReplyIndexItemDto>> GetReplyIndexItemDtoListByPostIdAndPageAsync(string postId, int page, int repliesPerPage)
        {
            return await _context.ReplyDataModels.Where(x => x.Post.Id == postId).Select(x => new ReplyIndexItemDto()
            {
                Id = x.Id,
                Content = x.Content,
                DateCreated = x.DateCreated,
                ImageUrl = x.User.ImageUrl,
                Likes = x.Likes,
                Role = x.User.Role,
                Username = x.User.UserName,
                UserId = x.User.Id
            }).Skip((page-1)*repliesPerPage).Take(repliesPerPage).ToListAsync();
        }
        public async Task<List<ReplyItemDto>> GetListReplyItemDtoByUsernameAsync(string username)
        {
            return await _context.ReplyDataModels.Where(x=>x.User.UserName == username).Select(x => new ReplyItemDto()
            {
                Content = x.Content,
                Id = x.Id,
                PostId = x.Post.Id,
                Created = x.DateCreated
            }).ToListAsync();
        }
        public async Task<ReplyDataModel> GetWithUserByIdAsync(string id)
        {
            return await _context.ReplyDataModels.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<string> GetPostIdById(string id)
        {
            return await _context.ReplyDataModels.Where(x => x.Id == id).Select(x => x.Post.Id).FirstOrDefaultAsync();
        }
        public async Task<List<ReplyDataModel>> GetAllWithUserByPostIdAsync(string postId)
        {
            return await _context.ReplyDataModels.Include(x => x.User).Where(x=>x.Post.Id==postId).ToListAsync();
        }
        public async Task<UserDataModel> GetUserByReplyIdAsync(string replyId)
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
        public async Task<string> GetUserIdByReplyIdAsync(string replyId)
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
        public async Task<bool> UpdateAsync(ReplyDataModel reply)
        {
            _context.ReplyDataModels.Update(reply);
            return true;
        }
        public async Task<DateTime> GetLastReplyDateTimeByUserIdAsync(string userId)
        {
            return await _context.ReplyDataModels.Where(x => x.User.Id == userId).Select(x => x.DateCreated).OrderByDescending(x => x.Date).FirstOrDefaultAsync();
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
        public async Task<bool> ExistByIdAsync(string id)
        {
            return await _context.ReplyDataModels.AnyAsync(x => x.Id == id);
        }
        public async Task DeleteRangeAsync(List<ReplyDataModel> replies)
        {
            _context.ReplyDataModels.RemoveRange(replies);
        }
    }
}
