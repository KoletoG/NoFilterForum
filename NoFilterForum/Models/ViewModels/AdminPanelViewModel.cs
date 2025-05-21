using NoFilterForum.Models.DataModels;

namespace NoFilterForum.Models.ViewModels
{
    public class AdminPanelViewModel
    {
        public List<UserDataModel> Users { get; set; }
        public bool HasReports { get; set; }
        public bool NotConfirmedExist { get; set; }
        public AdminPanelViewModel(List<UserDataModel> users, bool hasReports, bool notConfirmedExist) 
        {
            Users = users;
            HasReports = hasReports;
            NotConfirmedExist = notConfirmedExist;
        }
    }
}
