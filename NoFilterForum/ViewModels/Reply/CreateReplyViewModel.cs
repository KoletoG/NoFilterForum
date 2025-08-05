using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Reply
{
    public class CreateReplyViewModel
    {
        [Required]
        public required string PostId { get; set; }
        [Required]
        [MinLength(2)]
        [Display(Name ="Reply: ")]
        public string? Content { get; set; }
    }
}
