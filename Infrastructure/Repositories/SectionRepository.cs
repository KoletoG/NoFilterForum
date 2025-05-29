using Core.Models.DTOs.OutputDTOs;
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
        public async Task<SectionDataModel> GetByIdAsync(string id)
        {
            return await _context.SectionDataModels.FindAsync(id);
        }
        public async Task<SectionDataModel> GetWithPostsByTitleAsync(string title)
        {
            return await _context.SectionDataModels.Include(x=>x.Posts).FirstOrDefaultAsync(x => x.Title == title);
        }
        public async Task<List<SectionDataModel>> GetAllAsync()
        {
            return await _context.SectionDataModels.AsNoTracking().ToListAsync();
        }
        public async Task<List<PostItemDto>> GetPostItemsWithPagingByTitleAsync(string sectionTitle, int page, int countPerPage)
        {
            return await _context.SectionDataModels.AsNoTracking()
                    .Where(x => x.Title == sectionTitle)
                    .SelectMany(x => x.Posts)
                    .OrderByDescending(x => x.IsPinned)
                    .ThenByDescending(x => x.DateCreated)
                    .Skip((page - 1) * countPerPage)
                    .Take(countPerPage)
                    .Select(x=>new PostItemDto
                    {
                        DateCreated = x.DateCreated,
                        PostId = x.Id,
                        Role = x.User.Role,
                        Username = x.User.UserName
                    })
                    .ToListAsync();
        }
        public async Task<SectionDataModel> CreateAsync(SectionDataModel section)
        {
            await _context.SectionDataModels.AddAsync(section);
            return section;
        }
        public async Task UpdateAsync(SectionDataModel section)
        {
            _context.SectionDataModels.Update(section);
        }
        public async Task DeleteAsync(SectionDataModel section)
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
