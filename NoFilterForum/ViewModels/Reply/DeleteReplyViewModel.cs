using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Reply
{
    public class DeleteReplyViewModel
    {
        [Required]
        public required string ReplyId { get; set; }
        [Required]
        public required string PostId { get; set; }
    }
}
