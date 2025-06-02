using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Web.ViewModels.Section
{
    public class DeleteSectionViewModel
    {
        [Required]
        public string SectionId { get; set; }
    }
}
