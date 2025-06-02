using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Profile
{
    public class ChangeEmailViewModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
