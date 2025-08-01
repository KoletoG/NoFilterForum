using Core.Models.DTOs.OutputDTOs.Notification;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Infrastructure.Data;

namespace NoFilterForum.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
       
        public async Task<IReadOnlyCollection<NotificationDataModel>> GetAllByUserIdAsync(string userId)
        {
            return await _context.NotificationDataModels.Where(x => x.UserTo.Id == userId).ToListAsync();
        }
        public async Task<IReadOnlyCollection<NotificationDataModel>> GetAllByUserFromIdAsync(string userId)
        {
            return await _context.NotificationDataModels.Where(x => x.UserFrom.Id == userId).ToListAsync();
        }
        public async Task CreateRangeAsync(IEnumerable<NotificationDataModel> notifications)
        {
            await _context.NotificationDataModels.AddRangeAsync(notifications);
        }
        public async Task<IReadOnlyCollection<NotificationDataModel>> GetAllByReplyIdAsync(string replyId)
        {
            return await _context.NotificationDataModels.Where(x => x.Reply.Id == replyId).ToListAsync();
        }
        public async Task<IReadOnlyCollection<NotificationDataModel>> GetAllByReplyIdAsync(HashSet<string> replyIds)
        {
            return await _context.NotificationDataModels.Where(x => replyIds.Contains(x.Id)).ToListAsync();
        }
        public async Task<IReadOnlyCollection<NotificationsDto>> GetNotificationsAsDtoByUserIdAsync(string userId)
        {
            return await _context.NotificationDataModels.Where(x => x.UserTo.Id == userId)
                .Select(x => new NotificationsDto(x.Reply.Id,x.Reply.Post.Id,x.Reply.Post.Title,x.Reply.Content,x.UserFrom.UserName))
                .ToListAsync();
        }
        public void DeleteRange(IEnumerable<NotificationDataModel> notifications)
        {
            _context.NotificationDataModels.RemoveRange(notifications);
        }
    }
}
