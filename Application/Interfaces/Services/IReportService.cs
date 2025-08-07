using Core.Enums;
using Core.Models.DTOs.InputDTOs.Report;
using Core.Models.DTOs.OutputDTOs.Report;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IReportService
    {
        public Task<IReadOnlyCollection<ReportDataModel>> GetAllReportsAsync();
        public Task<bool> AnyReportsAsync(CancellationToken cancellationToken);
        public Task<IReadOnlyCollection<ReportItemDto>> GetAllDtosAsync(CancellationToken cancellationToken);
        public Task<PostResult> DeleteReportByIdAsync(DeleteReportRequest deleteReportRequest, CancellationToken cancellationToken); 
        public Task<PostResult> CreateReportAsync(CreateReportRequest createReportRequest, CancellationToken cancellationToken);
    }
}
