using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Web.ViewModels.Section
{
    public class DeleteSectionViewModel
    {
        [Required(ErrorMessage ="Section Id cannot be null")]
        public required string SectionId { get; set; }
    }
}
