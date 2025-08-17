using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Message
{
    public class DeleteMessageViewModel
    {
        [Required]
        public string Id { get; set; }
    }
}
