using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class ChangeEmailViewModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
