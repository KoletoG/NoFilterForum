using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Profile
{
    public class UpdateImageViewModel
    {
        [Required]
        public IFormFile? Image { get; set; }
    }
}
