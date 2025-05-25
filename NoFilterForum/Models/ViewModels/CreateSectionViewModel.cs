using System.ComponentModel.DataAnnotations;

namespace NoFilterForum.Models.ViewModels
{
    public class CreateSectionViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title cannot be empty.")]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title cannot be empty.")]
        [MinLength(10, ErrorMessage = "Description should be at least 10 characters long.")]
        public string Description { get; set; }
        public CreateSectionViewModel() { }
    }
}
