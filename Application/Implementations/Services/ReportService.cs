using System.Data;
using System.Net;
using Application.Interfaces.Services;
using Core.Enums;
using Core.Interfaces.Factories;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs.Report;
using Core.Models.DTOs.OutputDTOs.Report;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReportFactory _reportFactory;
        private readonly ILogger<ReportService> _logger;
        private readonly ICacheService _cacheService;
        public ReportService(IUnitOfWork unitOfWork,IReportFactory reportFactory, ILogger<ReportService> logger, ICacheService cacheService)
        { 
            _logger = logger;
            _reportFactory = reportFactory;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }
        public async Task<IReadOnlyCollection<ReportItemDto>> GetAllDtosAsync(CancellationToken cancellationToken) => await _cacheService.TryGetValue<IReadOnlyCollection<ReportItemDto>>("listReportItems", _unitOfWork.Reports.GetReportDtosAsync,cancellationToken) ?? [];
        public async Task<bool> AnyReportsAsync(CancellationToken cancellationToken) => await _unitOfWork.Reports.ExistsReportsAsync(cancellationToken);
        public async Task<PostResult> DeleteReportByIdAsync(DeleteReportRequest deleteReportRequest, CancellationToken cancellationToken)
        {
            var report = await _unitOfWork.Reports.GetByIdAsync(deleteReportRequest.ReportId);
            if (report is null)
            {
                return PostResult.NotFound;
            }
            try
            {
                await _unitOfWork.RunPOSTOperationAsync<ReportDataModel>(_unitOfWork.Reports.Delete, report,cancellationToken);
                return PostResult.Success;
            }
            catch(DBConcurrencyException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "A problem with database has occured when deleting report with Id: {ReportId}",deleteReportRequest.ReportId);
                return PostResult.UpdateFailed;
            }
            catch(OperationCanceledException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Operation was canceled");
                return PostResult.UpdateFailed;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, $"Report with Id: {deleteReportRequest.ReportId} wasn't deleted.");
                return PostResult.UpdateFailed;
            }
        }
        public async Task<PostResult> CreateReportAsync(CreateReportRequest createReportRequest, CancellationToken cancellationToken)
        {
            var userSentTo = await _unitOfWork.Users.GetByIdAsync(createReportRequest.UserToId);
            if(userSentTo is null)
            {
                return PostResult.NotFound;
            }
            var userSentFrom = await _unitOfWork.Users.GetByIdAsync(createReportRequest.UserFromId);
            if(userSentFrom is null)
            {
                return PostResult.NotFound;
            }
            var exists = createReportRequest.IsPost 
                ? await _unitOfWork.Posts.ExistByIdAsync(createReportRequest.IdOfPostOrReply, cancellationToken)
                : await _unitOfWork.Replies.ExistByIdAsync(createReportRequest.IdOfPostOrReply, cancellationToken);
            if (!exists)
            {
                return PostResult.NotFound;
            }
            var report = _reportFactory.CreateReport(createReportRequest.Content, userSentFrom, userSentTo, createReportRequest.IdOfPostOrReply,createReportRequest.IsPost);
            try
            {
                await _unitOfWork.RunPOSTOperationAsync<ReportDataModel>(_unitOfWork.Reports.CreateAsync, report, cancellationToken);
                return PostResult.Success;
            }
            catch (DBConcurrencyException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "A problem with database has occured when creating a report");
                return PostResult.UpdateFailed;
            }
            catch (OperationCanceledException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Operation was canceled");
                return PostResult.UpdateFailed;
            }
            catch (Exception ex) 
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Report by user with Id: {UserId} was not created", createReportRequest.UserFromId);
                return PostResult.UpdateFailed;
            }
        }
    }
}
