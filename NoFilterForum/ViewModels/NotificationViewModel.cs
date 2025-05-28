using System.Net;
using Core.Models.DTOs.OutputDTOs;
using NoFilterForum.Core.Models.DataModels;
using Web.ViewModels;

namespace NoFilterForum.Core.Models.ViewModels
{
    public class NotificationViewModel
    {
        public List<WarningItemViewModel> Warnings { get; set; }
        public List<NotificationItemViewModel> Notifications { get; set; }
        public NotificationViewModel(List<WarningItemViewModel> warnings,List<NotificationItemViewModel> notifications) 
        {
            Warnings = warnings;
            Notifications = notifications;
        }
    }
}
