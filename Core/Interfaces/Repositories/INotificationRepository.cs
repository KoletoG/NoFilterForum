using Core.Models.DTOs.OutputDTOs.Notification;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        public Task<IReadOnlyCollection<NotificationDataModel>> GetAllByReplyIdAsync(string replyId, CancellationToken cancellationToken); 
        public Task<IReadOnlyCollection<NotificationDataModel>> GetAllByReplyIdAsync(HashSet<string> replyIds, CancellationToken cancellationToken);
        public Task CreateRangeAsync(IEnumerable<NotificationDataModel> notifications,CancellationToken cancellationToken); 
        public Task<IReadOnlyCollection<NotificationDataModel>> GetAllByReplyIdsAsync(IEnumerable<string> repliesIds, CancellationToken cancellationToken);
        public Task<IReadOnlyCollection<NotificationsDto>> GetNotificationsAsDtoByUserIdAsync(string userId, CancellationToken cancellationToken);
        public void DeleteRange(IEnumerable<NotificationDataModel> notifications);
        public Task<IReadOnlyCollection<NotificationDataModel>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken);
    }
}
