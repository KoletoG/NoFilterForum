using System.ComponentModel.DataAnnotations;

namespace NoFilterForum.Core.Models.ViewModels
{
    public class CreateSectionViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title cannot be empty.")]
        [MinLength(8)]
        [Display(Name ="Title of the section")]
        public string? Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title cannot be empty.")]
        [MinLength(10, ErrorMessage = "Description should be at least 10 characters long.")]
        [Display(Name = "Description of the section")]
        public string? Description { get; set; }
        public CreateSectionViewModel() { }
    }
}
