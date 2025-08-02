using System.Linq;
using Core.Models.DTOs.InputDTOs.Post;
using Core.Models.DTOs.OutputDTOs.Post;
using Core.Models.DTOs.OutputDTOs.Section;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Infrastructure.Data;

namespace NoFilterForum.Infrastructure.Repositories
{
    public class SectionRepository : ISectionRepository
    {
        private readonly ApplicationDbContext _context;

        public SectionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> GetPostsCountByIdAsync(string id)
        {
            return await _context.SectionDataModels.Where(x => x.Id == id).Select(x => x.Posts).CountAsync();
        }
        public async Task<SectionDataModel?> GetByIdWithPostsAndRepliesAndUsersAsync(string id)
        {
            return await _context.SectionDataModels.Include(x => x.Posts).ThenInclude(x => x.Replies).ThenInclude(x => x.User).Include(x => x.Posts).ThenInclude(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<SectionDataModel?> GetWithPostsByTitleAsync(string title)
        {
            return await _context.SectionDataModels.Include(x => x.Posts).FirstOrDefaultAsync(x => x.Title == title);
        }
        public async Task<IReadOnlyCollection<SectionItemDto>> GetAllItemsDtoAsync()
        {
            return await _context.SectionDataModels.AsNoTracking()
                    .Select(x => new SectionItemDto(x.Title, x.Description, x.Id, x.Posts.Count))
                    .ToListAsync();
        }
        public async Task<IReadOnlyCollection<PostItemDto>> GetPostItemsWithPagingByTitleAsync(GetIndexPostRequest getIndexPostRequest)
        {
            return await _context.SectionDataModels.AsNoTracking()
                    .Where(x => x.Title == getIndexPostRequest.TitleOfSection)
                    .SelectMany(x => x.Posts)
                    .OrderByDescending(x => x.IsPinned)
                    .ThenByDescending(x => x.DateCreated)
                    .Skip((getIndexPostRequest.Page - 1) * getIndexPostRequest.PostsCount)
                    .Take(getIndexPostRequest.PostsCount)
                    .Select(x => new PostItemDto(x.Id, x.User.UserName, x.User.Role, x.Title, x.IsPinned, x.DateCreated, x.User.ImageUrl, x.Likes))
                    .ToListAsync();
        }
        public async Task<bool> ExistsSectionByTitleAsync(string sectionTitle)
        {
            return await _context.SectionDataModels.AnyAsync(x => x.Title == sectionTitle);
        }
        public async Task<SectionDataModel> CreateAsync(SectionDataModel section)
        {
            await _context.SectionDataModels.AddAsync(section);
            return section;
        }
        public void Update(SectionDataModel section)
        {
            _context.SectionDataModels.Update(section);
        }
        public void Delete(SectionDataModel section)
        {
            _context.SectionDataModels.Remove(section);
        }
        public async Task<int> GetPostsCountByTitleAsync(string title)
        {
            return await _context.SectionDataModels.Where(x => x.Title == title).SelectMany(x => x.Posts).CountAsync();
        }
        public async Task<bool> ExistsByTitleAsync(string title)
        {
            return await _context.SectionDataModels.AnyAsync(x => x.Title == title);
        }
    }
}
