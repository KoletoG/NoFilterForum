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
    }
}
