using NoFilterForum.Models.DataModels;

namespace NoFilterForum.Models.ViewModels
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
