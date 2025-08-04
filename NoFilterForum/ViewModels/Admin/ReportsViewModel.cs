using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Migrations;
using Web.ViewModels.Report;

namespace Web.ViewModels.Admin
{
    public class ReportsViewModel
    {
        public required IEnumerable<ReportItemViewModel> Reports { get; set; }
    }
}
