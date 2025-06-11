using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Admin
{
    public class ConfirmUserViewModel
    {
        [Required]
        public string UserId { get; set; }
    }
}
