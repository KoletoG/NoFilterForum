using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Profile
{
    public class ChangeImageViewModel
    {
        [Required]
        public IFormFile Image { get; set; }
    }
}
