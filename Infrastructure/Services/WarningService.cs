using Core.Enums;
using Core.Interfaces.Repositories;
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
        public WarningService(IUnitOfWork unitOfWork, ILogger<WarningService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<PostResult> AddWarningAsync(string content, UserDataModel user)
        {
            // Add sanitization of content
            var warning = new WarningDataModel(content, user);
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
    }
}
