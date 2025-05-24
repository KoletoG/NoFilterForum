using Microsoft.EntityFrameworkCore;
using NoFilterForum.Data;
using NoFilterForum.Models.DataModels;
using NoFilterForum.Repositories.Interfaces;
using NoFilterForum.Services.Interfaces;

namespace NoFilterForum.Repositories.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<NotificationDataModel> GetByIdAsync(string id)
        {
            return await _context.NotificationDataModels.FindAsync(id);
        }
        public async Task<List<NotificationDataModel>> GetAllAsync()
        {
            return await _context.NotificationDataModels.ToListAsync();
        }
        public async Task<NotificationDataModel> CreateAsync(NotificationDataModel notification)
        {
            await _context.NotificationDataModels.AddAsync(notification);
            await _context.SaveChangesAsync();
            return notification;
        }
        public async Task UpdateAsync(NotificationDataModel notification)
        {
            _context.NotificationDataModels.Update(notification);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(NotificationDataModel notification)
        {
            _context.NotificationDataModels.Remove(notification);
            await _context.SaveChangesAsync();
        }
    }
}
