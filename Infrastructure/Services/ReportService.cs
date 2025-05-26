using Core.Enums;
using Core.Interfaces.Repositories;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReportService> _logger;
        public ReportService(IUnitOfWork unitOfWork, ILogger<ReportService> logger)
        { 
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<ReportDataModel>> GetAllReportsAsync()
        {
            return await _unitOfWork.Reports.GetAllAsync();
        }
        public async Task<bool> AnyReportsAsync()
        {
            return await _unitOfWork.Reports.ExistsReportsAsync();
        }
        public async Task<PostResult> DeleteReportByIdAsync(string reportId)
        {
            var user = await _unitOfWork.Reports.GetByIdAsync(reportId);
            if (user==null)
            {
                return PostResult.NotFound;
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Reports.DeleteAsync(user);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return PostResult.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, $"Report with Id: {reportId} wasn't deleted.");
                return PostResult.UpdateFailed;
            }
        }
    }
}
