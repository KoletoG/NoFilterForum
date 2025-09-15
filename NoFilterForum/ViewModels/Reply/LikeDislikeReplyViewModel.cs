using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Reply
{
    public class LikeDislikeReplyViewModel
    {
        [Required]
        public string Id { get; set; }
    }
}
