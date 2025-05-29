using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class GetPostsViewModel
    {
        [DefaultValue(1)]
        public int Page { get; set; }
        [Required]
        public string TitleOfSection { get; set; }
        public string? Errors { get; set; }
    }
}
