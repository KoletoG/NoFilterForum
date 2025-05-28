using Core.Models.DTOs.OutputDTOs;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Models.ViewModels
{
    public class NotificationViewModel
    {
        public List<WarningsContentDto> Warnings { get; set; }
        public List<NotificationsDto> Notifications { get; set; }
        public NotificationViewModel(List<WarningsContentDto> warnings,List<NotificationsDto> notifications) 
        {
            Warnings = warnings;
            Notifications = notifications;
        }
    }
}
