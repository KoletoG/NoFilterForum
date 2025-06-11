using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.OutputDTOs.Notification;
using Microsoft.Extensions.Logging;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;

namespace NoFilterForum.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<NotificationService> _logger;
        public NotificationService(IUnitOfWork unitOfWork, ILogger<NotificationService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<PostResult> DeleteByUserIdAsync(string userId)
        {
            var notifications = await _unitOfWork.Notifications.GetAllByUserIdAsync(userId);
            if (!notifications.Any())
            {
                return PostResult.Success;
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Notifications.DeleteRangeAsync(notifications);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return PostResult.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Notifications from user with Id: {UserId} were not deleted", userId);
                return PostResult.UpdateFailed;
            }
        }
        public async Task<List<NotificationsDto>> GetNotificationsDtosByUserIdAsync(string userId)
        {
            return await _unitOfWork.Notifications.GetNotificationsAsDtoByUserIdAsync(userId);
        }            
    }
}
