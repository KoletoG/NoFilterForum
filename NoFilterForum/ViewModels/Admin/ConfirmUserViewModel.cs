using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Admin
{
    public class ConfirmUserViewModel
    {
        [Required]
        public required string UserId { get; set; }
    }
}
