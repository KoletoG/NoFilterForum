namespace NoFilterForum.Models.ViewModels
{
    public class NotificationViewModel
    {
        public List<WarningDataModel> Warnings { get; set; }
        public NotificationViewModel(List<WarningDataModel> warnings) 
        {
            Warnings = warnings;
        }
    }
}
