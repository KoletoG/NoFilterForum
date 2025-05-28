using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class ChangeUsernameViewModel
    {
        [Required]
        [MinLength(6)]
        [MaxLength(30)]
        public string Username { get; set; }
    }
}
