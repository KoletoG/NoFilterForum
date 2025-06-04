using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Profile
{
    public class ChangeBioViewModel
    {
        [Required]
        public string Bio {  get; set; }
    }
}
