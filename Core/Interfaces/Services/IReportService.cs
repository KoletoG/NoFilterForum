using Core.Enums;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IReportService
    {
        public Task<List<ReportDataModel>> GetAllReportsAsync();
        public Task<bool> AnyReportsAsync();
        public Task<PostResult> DeleteReportByIdAsync(string reportId);
    }
}
