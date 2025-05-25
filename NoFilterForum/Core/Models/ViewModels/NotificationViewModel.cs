using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Models.ViewModels
{
    public class NotificationViewModel
    {
        public List<WarningDataModel> Warnings { get; set; }
        public List<NotificationDataModel> Notifications { get; set; }
        public NotificationViewModel(List<WarningDataModel> warnings,List<NotificationDataModel> notifications) 
        {
            Warnings = warnings;
            Notifications = notifications;
        }
    }
}
