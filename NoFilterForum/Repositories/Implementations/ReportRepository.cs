using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Data;

namespace NoFilterForum.Repositories.Implementations
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ReportDataModel> GetByIdAsync(string id)
        {
            return await _context.ReportDataModels.FindAsync(id);
        }
        public async Task<List<ReportDataModel>> GetAllAsync()
        {
            return await _context.ReportDataModels.AsNoTracking().Include(x => x.UserTo).Include(x => x.UserFrom).ToListAsync();
        }
        public async Task<ReportDataModel> CreateAsync(ReportDataModel report)
        {
            await _context.ReportDataModels.AddAsync(report);
            await _context.SaveChangesAsync();
            return report;
        }
        public async Task UpdateAsync(ReportDataModel report)
        {
            _context.ReportDataModels.Update(report);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(ReportDataModel report)
        {
            _context.ReportDataModels.Remove(report);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ExistsReportsAsync()
        {
            return await _context.ReportDataModels.AnyAsync();
        }
    }
}
