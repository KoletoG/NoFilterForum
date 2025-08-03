using Core.Models.DTOs.OutputDTOs.Notification;
using Web.ViewModels.Notifications;

namespace Web.Mappers
{
    public static class NotificationMapper
    {
        public static NotificationItemViewModel MapToViewModel(NotificationsDto dto) => new()
        {
            ReplyContent = dto.ReplyContent,
            PostId = dto.PostId,
            PostTitle = dto.PostTitle,
            ReplyId = dto.ReplyId,
            UserFromUsername = dto.UserFromUsername
        };
    }
}
