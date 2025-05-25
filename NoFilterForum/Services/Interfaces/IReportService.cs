using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Services.Interfaces
{
    public interface IReportService
    {
        public Task<List<ReportDataModel>> GetAllReportsAsync();
        public Task<bool> AnyReportsAsync();
    }
}
