using System.Collections.Immutable;
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
        public async Task<IReadOnlyCollection<WarningDataModel>> GetAllByUserIdAsync(string userId, CancellationToken token)
        {
            return await _context.WarningDataModels.Where(x => x.User.Id == userId).ToListAsync(token);
        }
        public async Task<IReadOnlyCollection<WarningsContentDto>> GetWarningsContentAsDtoByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _context.WarningDataModels.AsNoTracking()
                .Where(x => x.User.Id == userId && !x.IsAccepted)
                .Select(x => new WarningsContentDto(x.Content))
                .ToListAsync(cancellationToken);
        }
        public async Task CreateAsync(WarningDataModel warning)
        {
            await _context.WarningDataModels.AddAsync(warning);
        }
        public void UpdateRange(IEnumerable<WarningDataModel> warnings)
        {
            _context.WarningDataModels.UpdateRange(warnings);
        }
        public async Task<IReadOnlyCollection<WarningsContentDto>> GetWarningsContentByUserIdAsync(string userId)
        {
            return await _context.WarningDataModels.Where(x => x.User.Id == userId)
                .Select(u => new WarningsContentDto(u.Content))
                .ToListAsync();
        }
    }
}
