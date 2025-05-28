using Core.Enums;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface INotificationService
    {
        public Task<PostResult> DeleteByUserIdAsync(string userId);
    }
}
