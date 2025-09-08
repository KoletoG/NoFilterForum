using System.Collections.ObjectModel;
using System.Data.Common;
using Application.Interfaces.Services;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.OutputDTOs.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<NotificationService> _logger;
        private readonly ICacheService _cacheService;
        public NotificationService(IUnitOfWork unitOfWork, ILogger<NotificationService> logger, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cacheService = cacheService;
        }
        // GET methods
        public async Task<int> GetNotificationsCountByUserIdAsync(string userId,CancellationToken cancellationToken)
        {
            return await _unitOfWork.Notifications.GetAll().Where(x=>x.UserToId==userId).CountAsync(cancellationToken);
        }
        public async Task<IReadOnlyCollection<NotificationsDto>> GetNotificationsDtosByUserIdAsync(string userId, CancellationToken cancellationToken) => await _cacheService.TryGetValue<IReadOnlyCollection<NotificationsDto>>($"listNotificationsDtoById_{userId}", _unitOfWork.Notifications.GetNotificationsAsDtoByUserIdAsync, userId, cancellationToken) ?? [];
        // POST methods
        public async Task<PostResult> DeleteByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            var notifications = await _unitOfWork.Notifications.GetAllByUserIdAsync(userId, cancellationToken);
            if (!notifications.Any())
            {
                return PostResult.Success;
            }
            try
            {
                await _unitOfWork.RunPOSTOperationAsync(_unitOfWork.Notifications.DeleteRange, notifications, cancellationToken);
                return PostResult.Success;
            }
            catch(OperationCanceledException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Deleting notifications of user with Id: {UserId} was canceled",userId);
                return PostResult.UpdateFailed;
            }
            catch (DbException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Notifications from user with Id: {UserId} were not deleted / Problem with DB", userId);
                return PostResult.UpdateFailed;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Notifications from user with Id: {UserId} were not deleted", userId);
                return PostResult.UpdateFailed;
            }
        }
    }
}
