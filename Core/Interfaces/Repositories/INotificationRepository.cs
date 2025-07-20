using Core.Models.DTOs.OutputDTOs.Notification;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        public Task<NotificationDataModel?> GetByIdAsync(string id);
        public Task<List<NotificationDataModel>> GetAllAsync();
        public Task<List<NotificationDataModel>> GetAllByReplyIdAsync(string replyId);
        public Task<List<NotificationDataModel>> GetAllByUserFromIdAsync(string userId);
        public Task<NotificationDataModel> CreateAsync(NotificationDataModel notification);
        public void Update(NotificationDataModel notification);
        public Task<bool> CreateRangeAsync(List<NotificationDataModel> notifications);
        public void Delete(NotificationDataModel notification);
        public Task<List<NotificationsDto>> GetNotificationsAsDtoByUserIdAsync(string userId);
        public void DeleteRange(List<NotificationDataModel> notifications);
        public Task<List<NotificationDataModel>> GetAllByUserIdAsync(string userId);
    }
}
