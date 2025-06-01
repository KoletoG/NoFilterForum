using System.Runtime.CompilerServices;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models.DTOs;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.OutputDTOs;
using Ganss.Xss;
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
        private readonly IHtmlSanitizer _htmlSanitizer;
        public WarningService(IUnitOfWork unitOfWork, ILogger<WarningService> logger, IHtmlSanitizer htmlSanitizer)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _htmlSanitizer = htmlSanitizer;
            _htmlSanitizer.AllowedTags.Clear();
        }
        public async Task<PostResult> AddWarningAsync(CreateWarningRequest createWarningRequest)
        {
            var user = await _unitOfWork.Users.GetUserWithWarningsByIdAsync(createWarningRequest.UserId);
            if (user == null)
            {
                return PostResult.NotFound;
            }
            createWarningRequest.Content = _htmlSanitizer.Sanitize(createWarningRequest.Content);
            var warning = new WarningDataModel(createWarningRequest.Content, user);
            user.Warnings.Add(warning);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return PostResult.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, $"Warning hasn't been added to user with Id: {user.Id}");
                return PostResult.UpdateFailed;
            }
        }
        public async Task<List<WarningsContentDto>> GetWarningsByUserIdAsync(string userId)
        {
            return await _unitOfWork.Warnings.GetWarningsContentByUserIdAsync(userId);
        }
        public async Task<List<WarningsContentDto>> GetWarningsContentDtosByUserIdAsync(string userId)
        {
            return await _unitOfWork.Warnings.GetWarningsContentAsDtoByUserIdAsync(userId);
        }
        public async Task<PostResult> AcceptWarningsAsync(string userId)
        {
            var warnings = await _unitOfWork.Warnings.GetAllByUserIdAsync(userId);
            if (warnings == null)
            {
                return PostResult.Success;
            }
            foreach (var warning in warnings)
            {
                warning.Accept();
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Warnings.UpdateRangeAsync(warnings);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return PostResult.Success;
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
