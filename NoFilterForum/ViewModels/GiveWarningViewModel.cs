using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class CreateWarningViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        [MinLength(10)]
        public string Content { get; set; }
    }
}
