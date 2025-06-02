using Core.Models.DTOs.OutputDTOs;
using Web.ViewModels.Notifications;

namespace Web.Mappers
{
    public static class NotificationMappers
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
