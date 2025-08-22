using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Message
{
    public class DeleteMessageViewModel
    {
        [Required]
        public string MessageId { get; set; }
        [Required]
        public string ChatId { get; set; }
    }
}
