using NoFilterForum.Models.DataModels;
using NoFilterForum.Repositories.Interfaces;
using NoFilterForum.Services.Interfaces;

namespace NoFilterForum.Services.Implementations
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
