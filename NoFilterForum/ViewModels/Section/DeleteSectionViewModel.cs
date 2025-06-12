using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Web.ViewModels.Section
{
    public class DeleteSectionViewModel
    {
        [Required(ErrorMessage ="Section Id cannot be null")]
        public string SectionId { get; set; }
    }
}
