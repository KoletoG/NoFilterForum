using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Reply
{
    public class CreateReplyViewModel
    {
        [Required]
        public string PostId { get; set; }
        [Required]
        [MinLength(2)]
        public string Content { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
