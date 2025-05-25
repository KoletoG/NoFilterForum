using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Repositories.Interfaces
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
