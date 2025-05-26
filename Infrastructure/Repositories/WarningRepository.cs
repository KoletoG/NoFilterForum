using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Infrastructure.Data;

namespace NoFilterForum.Infrastructure.Repositories
{
    public class WarningRepository : IWarningRepository
    {
        private readonly ApplicationDbContext _context;

        public WarningRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<WarningDataModel> GetByIdAsync(string id)
        {
            return await _context.WarningDataModels.FindAsync(id);
        }
        public async Task<List<WarningDataModel>> GetAllByUserAsync(UserDataModel user)
        {
            return await _context.WarningDataModels.Where(x=>x.User==user).ToListAsync();
        }
        public async Task<List<WarningDataModel>> GetAllAsync()
        {
            return await _context.WarningDataModels.ToListAsync();
        }
        public async Task<WarningDataModel> CreateAsync(WarningDataModel warning)
        {
            await _context.WarningDataModels.AddAsync(warning);
            return warning;
        }
        public async Task UpdateAsync(WarningDataModel warning)
        {
            _context.WarningDataModels.Update(warning);
        }
        public async Task DeleteAsync(WarningDataModel warning)
        {
            _context.WarningDataModels.Remove(warning);
        }
        public async Task<bool> ExistsByUserAsync(UserDataModel user)
        {
            return await _context.WarningDataModels.AnyAsync(x => x.User==user);
        }
    }
}
