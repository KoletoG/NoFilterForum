﻿using Core.Enums;
using Core.Models.DTOs.OutputDTOs.Notification;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface INotificationService
    {
        public Task<PostResult> DeleteByUserIdAsync(string userId);
        public Task<List<NotificationsDto>> GetNotificationsDtosByUserIdAsync(string userId);
    }
}
