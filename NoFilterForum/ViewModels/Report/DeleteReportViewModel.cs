using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Report
{
    public class DeleteReportViewModel
    {
        [Required]
        public string Id { get; set; }
    }
}
