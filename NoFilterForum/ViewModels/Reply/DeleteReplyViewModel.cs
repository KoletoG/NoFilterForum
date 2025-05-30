using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Reply
{
    public class DeleteReplyViewModel
    {
        [Required]
        public string ReplyId { get; set; }
        [Required]
        public string TitleOfSection { get; set; }
        [Required]
        public string PostId { get; set; }
    }
}
