using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Message
{
    public class CreateMessageViewModel
    {
        [Required]
        public string Message { get; set; }
        [Required]
        public string ChatId { get; set; }
    }
}
