using System.Runtime.CompilerServices;
using Core.Models.DTOs.OutputDTOs.Report;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Infrastructure.Data;

namespace NoFilterForum.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ReportDataModel?> GetByIdAsync(string id)
        {
            return await _context.ReportDataModels.FindAsync(id);
        }
        public async Task<IReadOnlyCollection<ReportItemDto>> GetReportDtosAsync(CancellationToken cancellationToken)
        {
            return await _context.ReportDataModels.AsNoTracking()
                .Select(x => new ReportItemDto(x.IdOfPostReply,x.UserTo.UserName!,x.UserFrom.UserName!,x.Content,x.Id))
                .ToListAsync(cancellationToken);
        }
        public async Task CreateAsync(ReportDataModel report, CancellationToken cancellationToken)
        {
            await _context.ReportDataModels.AddAsync(report,cancellationToken);
        }
        public void Delete(ReportDataModel report)
        {
            _context.ReportDataModels.Remove(report);
        }
        public async Task<bool> ExistsReportsAsync(CancellationToken cancellationToken)
        {
            return await _context.ReportDataModels.AnyAsync(cancellationToken);
        }
    }
}
