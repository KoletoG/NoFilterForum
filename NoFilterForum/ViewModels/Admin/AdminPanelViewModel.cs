using NoFilterForum.Core.Models.DataModels;
using Web.Controllers;
using Web.Static_variables;

namespace Web.ViewModels.Admin
{
    public class AdminPanelViewModel
    {
        public required IEnumerable<UserItemsAdminViewModel> Users { get; set; }
        public bool HasReports { get; set; }
        public bool NotConfirmedExist { get; set; }
        public int Page {  get; set; }
        public int TotalPages { get; set; }
        public string AdminControllerName => ControllerNames.AdminControllerName;
        public string ActionName => nameof(AdminController.Index);
    }
}
