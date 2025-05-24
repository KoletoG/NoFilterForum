using NoFilterForum.Models.DataModels;

namespace NoFilterForum.Repositories.Interfaces
{
    public interface IReportRepository
    {
        public Task<ReportDataModel> GetByIdAsync(string id);
        public Task<List<ReportDataModel>> GetAllAsync();
        public Task<ReportDataModel> CreateAsync(ReportDataModel report);
        public Task UpdateAsync(ReportDataModel report);
        public Task DeleteAsync(ReportDataModel report);
    }
}
