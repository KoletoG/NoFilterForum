using NoFilterForum.Core.Models.DataModels;

namespace Web.ViewModels.Admin
{
    public class AdminPanelViewModel
    {
        public List<UserItemsAdminViewModel> Users { get; set; }
        public bool HasReports { get; set; }
        public bool NotConfirmedExist { get; set; }
    }
}
