using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Post
{
    public class DeletePostViewModel
    {
        [Required]
        public string PostId { get; set; }
        [Required]
        public string SectionTitle { get; set; }
    }
}
