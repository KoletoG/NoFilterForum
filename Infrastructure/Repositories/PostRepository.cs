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
        public async Task<PostDataModel?> GetByIdAsync(string id)
        {
            return await _context.PostDataModels.FindAsync(id);
        }
        public async Task<string?> GetSectionTitleByIdAsync(string postId)
        {
            return await _context.PostDataModels.Where(x => x.Id == postId).Select(x => x.Section.Title).FirstOrDefaultAsync();
        }
        public async Task<PostDataModel?> GetWithRepliesByIdAsync(string id)
        {
            return await _context.PostDataModels.Where(x => x.Id == id)
                .Include(x => x.Replies)
                .FirstOrDefaultAsync();
        }
        public async Task<PostDataModel?> GetWithUserByIdAsync(string id)
        {
            return await _context.PostDataModels.Where(x => x.Id == id)
                .Include(x => x.User)
                .FirstOrDefaultAsync();
        }
        public async Task<PostReplyIndexDto?> GetPostReplyIndexDtoByIdAsync(string id)
        {
            return await _context.PostDataModels.AsNoTracking().Where(x => x.Id == id).Select(x => new PostReplyIndexDto(x.Id, x.User.Id, x.User.UserName, x.Likes, x.DateCreated, x.Title, x.Content, x.User.Role, x.User.ImageUrl)).FirstOrDefaultAsync();
        }
        public async Task<IReadOnlyCollection<ProfilePostDto>> GetListProfilePostDtoByUserIdAsync(string userId)
        {
            return await _context.PostDataModels.AsNoTracking()
                .Where(x => x.User.Id == userId)
                .Select(x => new ProfilePostDto(x.Id,x.Title,x.DateCreated))
                .ToListAsync();
        }
        public async Task<IReadOnlyCollection<PostDataModel>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _context.PostDataModels.Where(x => x.User.Id == userId).ToListAsync(cancellationToken);
        }
        public async Task<IReadOnlyCollection<PostDataModel>> GetAllAsync()
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
        public void Update(PostDataModel post)
        {
            _context.PostDataModels.Update(post);
        }
        public void UpdateRange(IEnumerable<PostDataModel> posts)
        {
            _context.PostDataModels.UpdateRange(posts);
        }
        public void Delete(PostDataModel post)
        {
            _context.PostDataModels.Remove(post);
        }
        public void DeleteRange(IEnumerable<PostDataModel> posts)
        {
            _context.PostDataModels.RemoveRange(posts);
        }
        public async Task<DateTime> GetLastPostDateByUsernameAsync(string userId)
        {
            return await _context.PostDataModels.AsNoTracking().Where(x => x.User.Id == userId).Select(x => x.DateCreated).OrderByDescending(x => x.Date).FirstOrDefaultAsync();
        }
        public async Task<bool> ExistByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _context.PostDataModels.AnyAsync(x=>x.Id == id, cancellationToken);
        }
    }
}
