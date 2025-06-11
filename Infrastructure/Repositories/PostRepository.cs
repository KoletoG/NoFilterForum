using Core.Models.DTOs.OutputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Reply;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Infrastructure.Data;

namespace NoFilterForum.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<PostDataModel> GetByIdAsync(string id)
        {
            return await _context.PostDataModels.FindAsync(id);
        }
        public async Task<string> GetSectionTitleByIdAsync(string postId)
        {
            return await _context.PostDataModels.Where(x => x.Id == postId).Select(x => x.Section.Title).FirstOrDefaultAsync();
        }
        public async Task<PostDataModel> GetWithRepliesByIdAsync(string id)
        {
            return await _context.PostDataModels.Where(x => x.Id == id)
                .Include(x => x.Replies)
                .FirstOrDefaultAsync();
        }
        public async Task<PostDataModel> GetWithUserByIdAsync(string id)
        {
            return await _context.PostDataModels.Where(x => x.Id == id)
                .Include(x => x.User)
                .FirstOrDefaultAsync();
        }
        public async Task<PostReplyIndexDto> GetPostReplyIndexDtoByIdAsync(string id)
        {
            return await _context.PostDataModels.Where(x => x.Id == id).Select(x => new PostReplyIndexDto()
            {
                Content = x.Content,
                Id = x.Id,
                DateCreated = x.DateCreated,
                Likes = x.Likes,
                Role = x.User.Role,
                Title = x.Title,
                UserId = x.User.Id,
                Username = x.User.UserName,
                ImageUrl = x.User.ImageUrl
            }).FirstOrDefaultAsync();
        }
        public async Task<List<ProfilePostDto>> GetListProfilePostDtoByUsernameAsync(string username)
        {
            return await _context.PostDataModels.Where(x => x.User.UserName == username).Select(x => new ProfilePostDto()
            {
                Id = x.Id,
                Title = x.Title,
                Created = x.DateCreated
            }).ToListAsync();
        }
        public async Task<List<PostDataModel>> GetAllByUserIdAsync(string userId)
        {
            return await _context.PostDataModels.Where(x => x.User.Id == userId).ToListAsync() ?? new List<PostDataModel>();
        }
        public async Task<List<PostDataModel>> GetAllAsync()
        {
            return await _context.PostDataModels.ToListAsync();
        }
        public async Task<PostDataModel> CreateAsync(PostDataModel post)
        {
            await _context.PostDataModels.AddAsync(post);
            return post;
        }
        public async Task<int> GetCountByPostIdAsync(string id)
        {
            return await _context.PostDataModels.Where(x => x.Id == id).CountAsync();
        }
        public async Task<bool> UpdateAsync(PostDataModel post)
        {
            // need error handling
            _context.PostDataModels.Update(post);
            return true;
        }
        public async Task<bool> UpdateRangeAsync(List<PostDataModel> posts)
        {
            _context.PostDataModels.UpdateRange(posts);
            return true;
        }
        public async Task DeleteAsync(PostDataModel post)
        {
            _context.PostDataModels.Remove(post);
        }
        public async Task DeleteRangeAsync(List<PostDataModel> posts)
        {
            _context.PostDataModels.RemoveRange(posts);
        }
        public async Task<DateTime> GetLastPostDateByUsernameAsync(string userId)
        {
            return await _context.PostDataModels.AsNoTracking().Where(x => x.User.Id == userId).Select(x => x.DateCreated).OrderByDescending(x => x.Date).FirstOrDefaultAsync();
        }
        public async Task<bool> ExistByIdAsync(string id)
        {
            return await _context.PostDataModels.AnyAsync(x=>x.Id == id);
        }
    }
}
