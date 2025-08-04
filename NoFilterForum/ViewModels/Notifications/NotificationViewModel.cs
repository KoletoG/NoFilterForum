using System.Net;
using Core.Models.DTOs.OutputDTOs;
using NoFilterForum.Core.Models.DataModels;
using Web.ViewModels.Warning;

namespace Web.ViewModels.Notifications
{
    public class NotificationViewModel
    {
        public IList<WarningItemViewModel> Warnings { get; set; }
        public IReadOnlyCollection<NotificationItemViewModel> Notifications { get; set; }
        public NotificationViewModel(IList<WarningItemViewModel> warnings,IReadOnlyCollection<NotificationItemViewModel> notifications) 
        {
            Warnings = warnings;
            Notifications = notifications;
        }
    }
}
