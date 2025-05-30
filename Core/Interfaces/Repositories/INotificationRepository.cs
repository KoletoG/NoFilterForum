using Core.Models.DTOs.OutputDTOs;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        public Task<NotificationDataModel> GetByIdAsync(string id);
        public Task<List<NotificationDataModel>> GetAllAsync();
        public Task<List<NotificationDataModel>> GetAllByReplyIdAsync(string replyId);
        public Task<NotificationDataModel> CreateAsync(NotificationDataModel notification);
        public Task UpdateAsync(NotificationDataModel notification);
        public Task DeleteAsync(NotificationDataModel notification);
        public Task<List<NotificationsDto>> GetNotificationsAsDtoByUserIdAsync(string userId);
        public Task DeleteRangeAsync(List<NotificationDataModel> notifications);
        public Task<List<NotificationDataModel>> GetAllByUserIdAsync(string userId);
    }
}
