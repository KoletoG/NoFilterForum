using Microsoft.EntityFrameworkCore;
using NoFilterForum.Data;
using NoFilterForum.Models.DataModels;
using NoFilterForum.Repositories.Interfaces;

namespace NoFilterForum.Repositories.Implementations
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
        public async Task<SectionDataModel> GetByTitleAsync(string title)
        {
            return await _context.SectionDataModels.FirstOrDefaultAsync(x => x.Title == title);
        }
        public async Task<List<SectionDataModel>> GetAllAsync()
        {
            return await _context.SectionDataModels.AsNoTracking().ToListAsync();
        }
        public async Task<SectionDataModel> CreateAsync(SectionDataModel section)
        {
            await _context.SectionDataModels.AddAsync(section);
            await _context.SaveChangesAsync();
            return section;
        }
        public async Task UpdateAsync(SectionDataModel section)
        {
            _context.SectionDataModels.Update(section);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(SectionDataModel section)
        {
            _context.SectionDataModels.Remove(section);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ExistsByTitleAsync(string title)
        {
            return await _context.SectionDataModels.AnyAsync(x => x.Title == title);
        }
    }
}
