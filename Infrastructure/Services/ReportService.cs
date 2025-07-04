﻿using Core.Enums;
using Core.Interfaces.Factories;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs.Report;
using Core.Models.DTOs.OutputDTOs.Report;
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
        private readonly IReportFactory _reportFactory;
        private readonly ILogger<ReportService> _logger;
        public ReportService(IUnitOfWork unitOfWork,IReportFactory reportFactory, ILogger<ReportService> logger)
        { 
            _logger = logger;
            _reportFactory = reportFactory;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<ReportDataModel>> GetAllReportsAsync()
        {
            return await _unitOfWork.Reports.GetAllAsync();
        }
        public async Task<List<ReportItemDto>> GetAllDtosAsync()
        {
            return await _unitOfWork.Reports.GetReportDtosAsync();
        }
        public async Task<bool> AnyReportsAsync()
        {
            return await _unitOfWork.Reports.ExistsReportsAsync();
        }
        public async Task<PostResult> DeleteReportByIdAsync(DeleteReportRequest deleteReportRequest)
        {
            var report = await _unitOfWork.Reports.GetByIdAsync(deleteReportRequest.ReportId);
            if (report == null)
            {
                return PostResult.NotFound;
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Reports.DeleteAsync(report);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return PostResult.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, $"Report with Id: {deleteReportRequest.ReportId} wasn't deleted.");
                return PostResult.UpdateFailed;
            }
        }
        public async Task<PostResult> CreateReportAsync(CreateReportRequest createReportRequest)
        {
            var userTo = await _unitOfWork.Users.GetByIdAsync(createReportRequest.UserToId);
            if(userTo == null)
            {
                return PostResult.NotFound;
            }
            var userFrom = await _unitOfWork.Users.GetByIdAsync(createReportRequest.UserFromId);
            if(userFrom == null)
            {
                return PostResult.NotFound;
            }
            if (createReportRequest.IsPost)
            {
                if(!await _unitOfWork.Posts.ExistByIdAsync(createReportRequest.IdOfPostOrReply))
                {
                    return PostResult.NotFound;
                }
            }
            else
            {
                if (!await _unitOfWork.Replies.ExistByIdAsync(createReportRequest.IdOfPostOrReply))
                {
                    return PostResult.NotFound;
                }
            }
            var report = _reportFactory.CreateReport(createReportRequest.Content,userFrom,userTo,createReportRequest.IdOfPostOrReply,createReportRequest.IsPost);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Reports.CreateAsync(report);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return PostResult.Success;
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
