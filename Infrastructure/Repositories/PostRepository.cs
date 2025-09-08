using Core.DTOs.OutputDTOs.Search;
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
        public IQueryable<PostDataModel> GetAll()
        {
            return _context.PostDataModels;
        }
        public async Task<PostDataModel?> GetByIdAsync(string id)
        {
            return await _context.PostDataModels.FindAsync(id);
        }
        public async Task<string?> GetSectionTitleByIdAsync(string postId, CancellationToken cancellationToken)
        {
            return await _context.PostDataModels.Where(x => x.Id == postId).Select(x => x.Section.Title).FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<PostDataModel?> GetWithRepliesByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _context.PostDataModels.Where(x => x.Id == id)
                .Include(x => x.Replies)
                .FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<PostDataModel?> GetWithUserByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _context.PostDataModels.Where(x => x.Id == id)
                .Include(x => x.User)
                .FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<PostReplyIndexDto?> GetPostReplyIndexDtoByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _context.PostDataModels.AsNoTracking().Where(x => x.Id == id).Select(x => new PostReplyIndexDto(x.Id, x.User.Id, x.User.UserName, x.Likes, x.DateCreated, x.Title, x.Content, x.User.Role, x.User.ImageUrl)).FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<IDictionary<string,ProfilePostDto>> GetListProfilePostDtoByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _context.PostDataModels.AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => new ProfilePostDto(x.Id,x.Title,x.DateCreated))
                .ToDictionaryAsync(x=>x.Id,cancellationToken);
        }
        public async Task<IReadOnlyCollection<PostDataModel>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _context.PostDataModels.Where(x => x.UserId == userId).ToListAsync(cancellationToken);
        }
        public async Task CreateAsync(PostDataModel post, CancellationToken cancellationToken)
        {
            await _context.PostDataModels.AddAsync(post, cancellationToken);
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
        public async Task<DateTime> GetLastPostDateByUsernameAsync(string userId, CancellationToken cancellationToken)
        {
            return await _context.PostDataModels.AsNoTracking().Where(x => x.UserId == userId).Select(x => x.DateCreated).OrderByDescending(x => x.Date).FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<bool> ExistByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _context.PostDataModels.AnyAsync(x=>x.Id == id, cancellationToken);
        }
    }
}
