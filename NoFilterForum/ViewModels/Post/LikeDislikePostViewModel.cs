using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Post
{
    public class LikeDislikePostViewModel
    {
        [Required]
        public required string Id { get; set; }
    }
}
