using NoFilterForum.Core.Models.DataModels;

namespace Web.ViewModels.Admin
{
    public class AdminPanelViewModel
    {
        public List<UserItemsAdminViewModel> Users { get; set; }
        public bool HasReports { get; set; }
        public bool NotConfirmedExist { get; set; }
        public AdminPanelViewModel(List<UserItemsAdminViewModel> users, bool hasReports, bool notConfirmedExist) 
        {
            Users = users;
            HasReports = hasReports;
            NotConfirmedExist = notConfirmedExist;
        }
    }
}
