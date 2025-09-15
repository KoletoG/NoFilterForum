using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Report
{
    public class DeleteReportViewModel
    {
        public required string Id { get; init; }
        public DeleteReportViewModel()
        {
        }
    }
}
