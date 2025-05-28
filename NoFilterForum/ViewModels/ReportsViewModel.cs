using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Models.ViewModels
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
