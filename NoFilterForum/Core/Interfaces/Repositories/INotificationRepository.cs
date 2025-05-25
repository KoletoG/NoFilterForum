using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        public Task<NotificationDataModel> GetByIdAsync(string id);
        public Task<List<NotificationDataModel>> GetAllAsync();
        public Task<NotificationDataModel> CreateAsync(NotificationDataModel notification);
        public Task UpdateAsync(NotificationDataModel notification);
        public Task DeleteAsync(NotificationDataModel notification);
    }
}
