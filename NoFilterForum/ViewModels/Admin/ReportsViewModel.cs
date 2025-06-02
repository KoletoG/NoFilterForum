using NoFilterForum.Core.Models.DataModels;

namespace Web.ViewModels.Admin
{
    public class ReportsViewModel
    {
        public List<ReportDataModel> Reports { get; set; }
        public ReportsViewModel(List<ReportDataModel> reports)
        {
            Reports = reports;
        }
    }
}
