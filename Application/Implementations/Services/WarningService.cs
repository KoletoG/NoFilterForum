using System.Data.Common;
using System.Runtime.CompilerServices;
using Application.Interfaces.Services;
using Core.Enums;
using Core.Interfaces.Factories;
using Core.Interfaces.Repositories;
using Core.Models.DTOs;
using Core.Models.DTOs.InputDTOs.Warning;
using Core.Models.DTOs.OutputDTOs.Warning;
using Ganss.Xss;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Infrastructure.Services
{
    public class WarningService : IWarningService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<WarningService> _logger;
        private readonly IWarningFactory _warningFactory;
        private readonly ICacheService _cacheService;
        public WarningService(IUnitOfWork unitOfWork,IWarningFactory warningFactory, ILogger<WarningService> logger, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _warningFactory = warningFactory;
            _logger = logger;
            _cacheService = cacheService;
        }
        // GET methods
        public async Task<List<WarningsContentDto>> GetWarningsContentDtosByUserIdAsync(string userId) => await _cacheService.TryGetValue<List<WarningsContentDto>>($"listWarningContentsById_{userId}", _unitOfWork.Warnings.GetWarningsContentAsDtoByUserIdAsync, userId) ?? new();
        // POST methods
        public async Task<PostResult> AddWarningAsync(CreateWarningRequest createWarningRequest)
        {
            var user = await _unitOfWork.Users.GetUserWithWarningsByIdAsync(createWarningRequest.UserId);
            if (user is null)
            {
                return PostResult.NotFound;
            }
            var warning = _warningFactory.Create(createWarningRequest.Content, user);
            user.Warnings.Add(warning);
            try
            {
                await _unitOfWork.RunPOSTOperationAsync<UserDataModel>(_unitOfWork.Users.Update, user);
                return PostResult.Success;
            }
            catch (DbException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Warning hasn't been added to user with Id: {UserId} / Problem with DB", user.Id);
                return PostResult.UpdateFailed;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Warning hasn't been added to user with Id: {UserId}",user.Id);
                return PostResult.UpdateFailed;
            }
        }
       public async Task<PostResult> AcceptWarningsAsync(string userId)
        {
            var warnings = await _unitOfWork.Warnings.GetAllByUserIdAsync(userId);
            if (warnings is null)
            {
                return PostResult.Success;
            }
            foreach (var warning in warnings)
            {
                warning.Accept();
            }
            try
            {
                await _unitOfWork.RunPOSTOperationAsync<WarningDataModel>(_unitOfWork.Warnings.UpdateRange, warnings);
                return PostResult.Success;
            }
            catch(DbException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Warnings of user with Id: {UserId} were not accepted / Problem with DB", userId);
                return PostResult.UpdateFailed;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Warnings of user with Id: {UserId} were not accepted", userId);
                return PostResult.UpdateFailed;
            }
        }
    }
}
