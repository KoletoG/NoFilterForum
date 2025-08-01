using Core.Models.DTOs.OutputDTOs.Notification;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        public Task<IReadOnlyCollection<NotificationDataModel>> GetAllByReplyIdAsync(string replyId); 
        public Task<IReadOnlyCollection<NotificationDataModel>> GetAllByReplyIdAsync(HashSet<string> replyIds);
        public Task<IReadOnlyCollection<NotificationDataModel>> GetAllByUserFromIdAsync(string userId);
        public Task CreateRangeAsync(IEnumerable<NotificationDataModel> notifications);
        public Task<IReadOnlyCollection<NotificationsDto>> GetNotificationsAsDtoByUserIdAsync(string userId);
        public void DeleteRange(IEnumerable<NotificationDataModel> notifications);
        public Task<IReadOnlyCollection<NotificationDataModel>> GetAllByUserIdAsync(string userId);
    }
}
