using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Reply
{
    public class LikeDislikeReplyViewModel
    {
        [Required]
        public required string Id { get; set; }
    }
}
