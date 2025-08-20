using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Chat
{
    public class UpdateLastMessageViewModel
    {
        [Required]
        public string ChatId { get; set; }
        [Required]
        public string MessageId {  get; set; }
    }
}
