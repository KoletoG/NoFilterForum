using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Chat
{
    public class CreateChatViewModel
    {
        [Required]
        public required string UserId { get; set; }
    }
}
