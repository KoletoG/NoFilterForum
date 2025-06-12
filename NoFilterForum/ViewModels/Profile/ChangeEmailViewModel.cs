using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Profile
{
    public class ChangeEmailViewModel
    {
        [EmailAddress]
        [Required]
        [Display(Name ="Email")]
        public string Email { get; set; }
    }
}
