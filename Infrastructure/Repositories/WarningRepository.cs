using Core.Models.DTOs;
using Core.Models.DTOs.OutputDTOs.Warning;
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
        public async Task<WarningDataModel?> GetByIdAsync(string id)
        {
            return await _context.WarningDataModels.FindAsync(id);
        }
        public async Task<List<WarningDataModel>> GetAllByUserIdAsync(string userId)
        {
            return await _context.WarningDataModels.Where(x => x.User.Id == userId).ToListAsync();
        }
        public async Task<List<WarningDataModel>> GetAllAsync()
        {
            return await _context.WarningDataModels.ToListAsync();
        }
        public async Task<List<WarningsContentDto>> GetWarningsContentAsDtoByUserIdAsync(string userId)
        {
            return await _context.WarningDataModels.Where(x => x.User.Id == userId && !x.IsAccepted)
                .Select(x => new WarningsContentDto(x.Content)).ToListAsync();
        }
        public async Task<WarningDataModel> CreateAsync(WarningDataModel warning)
        {
            await _context.WarningDataModels.AddAsync(warning);
            return warning;
        }
        public void Update(WarningDataModel warning)
        {
            _context.WarningDataModels.Update(warning);
        }
        public void UpdateRange(List<WarningDataModel> warnings)
        {
            _context.WarningDataModels.UpdateRange(warnings);
        }
        public void Delete(WarningDataModel warning)
        {
            _context.WarningDataModels.Remove(warning);
        }
        public async Task<bool> ExistsByUserAsync(UserDataModel user)
        {
            return await _context.WarningDataModels.AnyAsync(x => x.User == user);
        }
        public async Task<List<WarningsContentDto>> GetWarningsContentByUserIdAsync(string userId)
        {
            return await _context.WarningDataModels.Where(x => x.User.Id == userId)
                .Select(u => new WarningsContentDto(u.Content)).ToListAsync();
        }
    }
}
