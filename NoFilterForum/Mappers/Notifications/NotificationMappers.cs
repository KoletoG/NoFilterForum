using Core.Models.DTOs.OutputDTOs;
using Web.ViewModels;

namespace Web.Mappers.Notifications
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
