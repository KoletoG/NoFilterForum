using Core.Enums;
using Core.Models.DTOs.OutputDTOs.Report;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface IReportRepository
    {
        public Task<ReportDataModel?> GetByIdAsync(string id);
        public Task<IReadOnlyCollection<ReportDataModel>> GetAllAsync();
        public Task CreateAsync(ReportDataModel report, CancellationToken cancellationToken);
        public void Update(ReportDataModel report);
        public void Delete(ReportDataModel report);
        public Task<IReadOnlyCollection<ReportItemDto>> GetReportDtosAsync(CancellationToken cancellationToken);
        public Task<bool> ExistsReportsAsync();
    }
}
