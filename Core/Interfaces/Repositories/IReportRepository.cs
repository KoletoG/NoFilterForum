using Core.Enums;
using Core.Models.DTOs.OutputDTOs.Report;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface IReportRepository
    {
        public Task<ReportDataModel?> GetByIdAsync(string id);
        public Task<List<ReportDataModel>> GetAllAsync();
        public Task<ReportDataModel> CreateAsync(ReportDataModel report);
        public Task UpdateAsync(ReportDataModel report);
        public Task DeleteAsync(ReportDataModel report);
        public Task<List<ReportItemDto>> GetReportDtosAsync();
        public Task<bool> ExistsReportsAsync();
    }
}
