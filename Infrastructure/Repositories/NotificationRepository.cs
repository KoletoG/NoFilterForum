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
        public async Task<NotificationDataModel?> GetByIdAsync(string id)
        {
            return await _context.NotificationDataModels.FindAsync(id);
        }
        public async Task<List<NotificationDataModel>> GetAllAsync()
        {
            return await _context.NotificationDataModels.ToListAsync();
        }
        public async Task<List<NotificationDataModel>> GetAllByUserIdAsync(string userId)
        {
            return await _context.NotificationDataModels.Where(x => x.UserTo.Id == userId).ToListAsync();
        }
        public async Task<List<NotificationDataModel>> GetAllByUserFromIdAsync(string userId)
        {
            return await _context.NotificationDataModels.Where(x=>x.UserFrom.Id == userId).ToListAsync();
        }
        public async Task<NotificationDataModel> CreateAsync(NotificationDataModel notification)
        {
            await _context.NotificationDataModels.AddAsync(notification);
            return notification;
        }
        public async Task<bool> CreateRangeAsync(List<NotificationDataModel> notifications)
        {
            await _context.NotificationDataModels.AddRangeAsync(notifications);
            return true;
        }
        public async Task<List<NotificationDataModel>> GetAllByReplyIdAsync(string replyId)
        {
            return await _context.NotificationDataModels.Where(x => x.Reply.Id == replyId).ToListAsync();
        }
        public async Task<List<NotificationDataModel>> GetAllByReplyIdAsync(HashSet<string> replyIds)
        {
            return await _context.NotificationDataModels.Where(x => replyIds.Contains(x.Id)).ToListAsync();
        }
        public void Update(NotificationDataModel notification)
        {
            _context.NotificationDataModels.Update(notification);
        }
        public void Delete(NotificationDataModel notification)
        {
            _context.NotificationDataModels.Remove(notification);
        }
        public async Task<List<NotificationsDto>> GetNotificationsAsDtoByUserIdAsync(string userId)
        {
            return await _context.NotificationDataModels.Where(x => x.UserTo.Id == userId)
                .Select(x => new NotificationsDto(x.Reply.Id,x.Reply.Post.Id,x.Reply.Post.Title,x.Reply.Content,x.UserFrom.UserName))
                .ToListAsync();
        }
        public void DeleteRange(List<NotificationDataModel> notifications)
        {
            _context.NotificationDataModels.RemoveRange(notifications);
        }
    }
}
