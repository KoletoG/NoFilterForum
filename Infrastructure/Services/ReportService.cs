using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        public ReportService(IReportRepository reportRepository)
        { 
            _reportRepository = reportRepository; 
        }
        public async Task<List<ReportDataModel>> GetAllReportsAsync()
        {
            return await _reportRepository.GetAllAsync();
        }
        public async Task<bool> AnyReportsAsync()
        {
            return await _reportRepository.ExistsReportsAsync();
        }
    }
}
