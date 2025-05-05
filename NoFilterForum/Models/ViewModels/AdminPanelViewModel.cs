namespace NoFilterForum.Models.ViewModels
{
    public class AdminPanelViewModel
    {
        public List<UserDataModel> Users { get; set; }
        public bool HasReports { get; set; }
        public AdminPanelViewModel(List<UserDataModel> users, bool hasReports) 
        {
            Users = users;
            HasReports = hasReports;
        }
    }
}
