using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Profile
{
    public class ChangeUsernameViewModel
    {
        [Required]
        [MinLength(6,ErrorMessage ="Name should be at least 6 characters")]
        [MaxLength(30, ErrorMessage = "Name should be at maximum 30 characters")]
        public string Username { get; set; }
        public ChangeUsernameViewModel()
        {
            Username = string.Empty;
        }
    }
}
