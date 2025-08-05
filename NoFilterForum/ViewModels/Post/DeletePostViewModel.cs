using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Post
{
    public class DeletePostViewModel
    {
        [Required]
        public required string PostId { get; set; }
    }
}
