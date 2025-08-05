using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Profile
{
    public class ChangeBioViewModel
    {
        [Required]
        [MaxLength(170,ErrorMessage ="Bio cannot be longer than 170 characters")]
        public string Bio {  get; set; }
        public ChangeBioViewModel()
        {
            Bio = string.Empty;
        }
    }
}
